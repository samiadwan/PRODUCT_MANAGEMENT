using DataAccessLayer.AccessLayer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.AccessLayer.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .IsRequired();
            builder.HasData(new User { Id = 1, Name = "John Doe", Email = "john.doe@gmail.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@gmail.com" });
        }
    }
}
