using DataAccessLayer.AccessLayer.Models;

namespace ProductManagement.DTOs
{
    public class OrderItemDto
    {
        public int? OrderId { get; set; }
        public Order? Order { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
