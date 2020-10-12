using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly TodoContext _context;

        public TodoItemService(TodoContext context)
        {
            _context = context;
        }

        public async Task AddItemAsync(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(TodoItem item)
        {
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<TodoItem> GetItemAsync(long id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.AsNoTracking();
        }

        public async Task UpdateItemAsync(TodoItem item)
        {
            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<bool> TodoItemExistsAsync(long id)
        {
            return await _context.TodoItems.AnyAsync(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
