using DataAccessLayer.AccessLayer;
using DataAccessLayer.AccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task CreateOrderAsync(Order order);
        Task SaveChangesAsync();
    }

    public class OrderRepository : IOrderRepository
    {
        private ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.Include(o => o.User).Include(o => o.OrderItems).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.User).Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }
        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
