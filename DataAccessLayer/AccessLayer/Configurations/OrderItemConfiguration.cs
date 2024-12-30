using DataAccessLayer.AccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.AccessLayer.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => new { oi.OrderId, oi.ProductId });

            builder.Property(oi => oi.Quantity)
                  .IsRequired();

            builder .HasOne(oi => oi.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(oi => oi.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
            builder.HasData(
                new OrderItem
                {
                    OrderId = 1, 
                    ProductId = 1, 
                    Quantity = 2
                },
                new OrderItem
                {
                    OrderId = 1,
                    ProductId = 2, 
                    Quantity = 1
                }
            );
        }
    }
}
