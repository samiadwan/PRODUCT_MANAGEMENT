using DataAccessLayer.AccessLayer;
using DataAccessLayer.AccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Repositories
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetOrderItemAsync();
        Task<OrderItem> GetOrderItemByIdAsync(int orderId, int productId);
        Task CreateOrderItemAsync(OrderItem item);
        Task SaveChangesAsync();
    }

    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemAsync()
        {
           return await _context.OrderItem.ToListAsync();
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int orderId, int productId)
        {
            return await _context.OrderItem.FindAsync(orderId, productId);
        }
        public async Task CreateOrderItemAsync(OrderItem item)
        {
            await _context.OrderItem.AddAsync(item);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
