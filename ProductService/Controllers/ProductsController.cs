using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductsController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return product;
        }

        // PUT: api/Products/5/stock
        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] StockUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            if (product.Stock < request.Quantity)
                return BadRequest("Not enough stock");

            // Update stock
            product.Stock -= request.Quantity;
            await _context.SaveChangesAsync();

            // Log stock update (this simulates async messaging)
            System.Console.WriteLine($"STOCK-UPDATE-EVENT: ProductId={id}, NewStock={product.Stock}");

            return Ok(new { NewStock = product.Stock });
        }
    }

    public class StockUpdateRequest
    {
        public int Quantity { get; set; }
    }
}