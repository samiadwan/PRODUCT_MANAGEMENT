using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.AccessLayer.Models
{
        public class Product
        {
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }
            [Required]
            [Range(0, double.MaxValue)]
            public decimal Price { get; set; }

            public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        }  
}
