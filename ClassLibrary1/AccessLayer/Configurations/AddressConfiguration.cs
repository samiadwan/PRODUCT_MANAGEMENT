using DataAccessLayer.AccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.AccessLayer.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(a => a.Street)
                     .IsRequired();

            builder.Property(a => a.City)
                  .IsRequired();

            builder.Property(a => a.PostalCode)
            .IsRequired();

            builder.HasOne(a => a.User)
                        .WithOne(u => u.Address)
                        .HasForeignKey<Address>(a => a.UserId);
            builder.HasData(
                new Address
                {
                    Id = 1,
                    Street = "123 Main St",
                    City = "Springfield",
                    PostalCode = "12345",
                    UserId = 1 
                },
                new Address
                {
                    Id = 2,
                    Street = "456 Elm St",
                    City = "Shelbyville",
                    PostalCode = "67890",
                    UserId = 2 
                }
            );
        }
    }
}
