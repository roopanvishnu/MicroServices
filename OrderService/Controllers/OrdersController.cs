using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using OrderService.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly ProductServiceClient _productService;

        public OrdersController(OrderDbContext context, ProductServiceClient productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.Include(o => o.Items).ToListAsync();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            if (order.Items == null || order.Items.Count == 0)
            {
                return BadRequest("Order must have at least one item");
            }

            decimal total = 0;
            order.OrderDate = DateTime.Now;

            // Process each order item using synchronous communication
            foreach (var item in order.Items)
            {
                // Get product details (synchronous HTTP call)
                var product = await _productService.GetProductAsync(item.ProductId);
                if (product == null)
                {
                    return BadRequest($"Product {item.ProductId} not found");
                }

                // Set product details
                item.ProductName = product.Name;
                item.ProductPrice = product.Price;
                total += product.Price * item.Quantity;
            }

            order.TotalAmount = total;

            // Save order
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Update product stock (synchronous HTTP call)
            foreach (var item in order.Items)
            {
                bool result = await _productService.UpdateStockAsync(item.ProductId, item.Quantity);
                if (!result)
                {
                    Console.WriteLine($"Failed to update stock for product {item.ProductId}");
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }
}