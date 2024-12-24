using DataAccessLayer.AccessLayer;
using DataAccessLayer.AccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>>GetProductsAsync();
        Task<Product>GetProductByIdAsync(int id);
        Task CreateProductAsync(Product product);
        Task SaveChangesAsync();

    }
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
        public Task<Product> GetProductByIdAsync(int id)
        {
            return _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
