using OrderService.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderService.Services
{
    // Client for synchronous communication with ProductService
    public class ProductServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ProductServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = "https://localhost:5001/api/products";
        }

        // Get product details 
        public async Task<ProductDto> GetProductAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ProductDto>($"{_baseUrl}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product: {ex.Message}");
                return null;
            }
        }

        // Update product stock
        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(new { quantity }),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PutAsync(
                    $"{_baseUrl}/{productId}/stock",
                    content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock: {ex.Message}");
                return false;
            }
        }
    }
}