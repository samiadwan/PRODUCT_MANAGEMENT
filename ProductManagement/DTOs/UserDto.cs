using DataAccessLayer.AccessLayer.Models;

namespace ProductManagement.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Address? Address { get; set; }

        public ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}
