using DataAccessLayer.AccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.AccessLayer.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                      .IsRequired();

            builder.Property(p => p.Price)
                  .IsRequired();
            builder.HasData(new Product { Id=1, Name = "Headphone", Price = 999 },
              new Product { Id=2, Name = "Watch", Price = 5000 });
        }
    }
}
