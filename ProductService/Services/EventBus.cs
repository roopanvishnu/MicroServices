using System;
using System.Threading.Tasks;

namespace ProductService.Services
{
    // Message class
    public class StockUpdateMessage
    {
        public int ProductId { get; set; }
        public int NewStock { get; set; }
    }

    // Simple event bus
    public class EventBus
    {
        // Singleton pattern
        private static EventBus _instance;
        public static EventBus Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventBus();
                return _instance;
            }
        }

        // Event for stock updates
        public event EventHandler<StockUpdateMessage> StockUpdated;

        // Method to publish stock updates
        public Task PublishStockUpdateAsync(StockUpdateMessage message)
        {
            Console.WriteLine($"[ProductService] Published: ProductId={message.ProductId}, Stock={message.NewStock}");
            StockUpdated?.Invoke(this, message);
            return Task.CompletedTask;
        }
    }
}