using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.AccessLayer.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public Address? Address { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
