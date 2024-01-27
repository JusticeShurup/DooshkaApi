using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ToDoItemRepository : IRepository<ToDoItem>
    {
        private readonly ApplicationContext _context;

        public ToDoItemRepository(ApplicationContext context) 
        {
            _context = context;
        }

        public async Task CreateAsync(ToDoItem item)
        {
            await _context.ToDoItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            
            if (result == null)
            {
                return;
            }

            _context.Remove(result!);
            await _context.SaveChangesAsync();
        
        }

        public IEnumerable<ToDoItem> Find(Func<ToDoItem, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ToDoItem?> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ToDoItem?> FindByIdAsync(Guid id)
        {
            return await _context.ToDoItems.FirstOrDefaultAsync(p => p.Id == id);
        }

        public ToDoItem Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ToDoItem> GetAll()
        {
            return _context.ToDoItems;
        }

        public async Task<IEnumerable<ToDoItem>> GetAllByCondition(Func<ToDoItem, bool> predicate)
        {
            return await _context.ToDoItems.ToListAsync();
        }

        public async Task UpdateAsync(ToDoItem item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();

        }
    }
}
