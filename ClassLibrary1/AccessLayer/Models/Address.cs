using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.AccessLayer.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

    }
}
