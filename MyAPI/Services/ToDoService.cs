using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAPI.Services
{
    public class ToDoService : IToDoService
    {
        private readonly TodoContext _context;

        public ToDoService(TodoContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>> GetTodoItems() => await _context.TodoItems.ToListAsync();


        public async Task<TodoItem> GetTodoItem(long id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task UpdateToDoItem(long id, TodoItem todoItem)
        {
            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddItem(TodoItem todoItem) {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(TodoItem todoItem)
        {
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
        }
    }
}
