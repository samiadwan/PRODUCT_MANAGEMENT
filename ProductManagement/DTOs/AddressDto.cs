using DataAccessLayer.AccessLayer.Models;

namespace ProductManagement.DTOs
{
    public class AddressDTO
    {
        
            public int Id { get; set; }

            public string Street { get; set; }

            public string City { get; set; }

            public string PostalCode { get; set; }

            public int? UserId { get; set; }

            public User? User { get; set; }

        
    }
}
