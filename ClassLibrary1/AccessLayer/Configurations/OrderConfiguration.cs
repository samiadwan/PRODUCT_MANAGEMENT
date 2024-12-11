using DataAccessLayer.AccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.AccessLayer.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.OrderDate)
                     .IsRequired();

            builder.HasOne(o => o.User)
                  .WithMany(u => u.Orders)
                  .HasForeignKey(o => o.UserId);
            builder.HasData(
                new Order
                {
                    Id = 1,
                    OrderDate = new DateTime(2024, 12, 10),
                    UserId = 1
                },
                new Order
                {
                    Id = 2,
                    OrderDate = new DateTime(2024, 12, 11),
                    UserId = 2 
                }
            );
        }
    }
}
