using DataAccessLayer.AccessLayer.Models;

namespace ProductManagement.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
