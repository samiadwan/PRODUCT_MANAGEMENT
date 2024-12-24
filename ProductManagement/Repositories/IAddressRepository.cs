using DataAccessLayer.AccessLayer;
using DataAccessLayer.AccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Repositories
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAddressesAsync();
        Task<Address> GetAddressByIdAsync(int id);
        Task CreateAddressAsync(Address address);
        Task SaveChangesAsync();
    }
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;
        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public async Task<IEnumerable<Address>> GetAddressesAsync()
        {
            return await _context.Addresses.Include(a => a.User).ToListAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int id)
        {
            return await _context.Addresses.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task CreateAddressAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
