using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApi.Services
{
    public interface ITodoItemService : IDisposable
    {
        IEnumerable<TodoItem> GetAll();
        Task<TodoItem> GetItemAsync(long id);
        Task AddItemAsync(TodoItem item);
        Task UpdateItemAsync(TodoItem item);
        Task DeleteItemAsync(TodoItem item);
        Task<bool> TodoItemExistsAsync(long id);
    }
}
