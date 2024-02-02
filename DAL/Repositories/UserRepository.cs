using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(User item)
        {
            await _context.Users.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User item)
        {
            _context.Users.Update(item);
            await _context.SaveChangesAsync();
        }

        public User? Find(Func<User, bool> predicate)
        {
            return _context.Users.SingleOrDefault(predicate);
        }

        public async Task<IEnumerable<User>> FindAll(Func<User, bool> predicate)
        {
            return await Task.FromResult(_context.Users.Where(predicate).ToList());
        }

        public async Task DeleteAsync(User item)
        {
            _context.Users.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
