using Microsoft.AspNetCore.Mvc;
using MyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAPI.Services
{
    public interface IToDoService
    {
        Task<List<TodoItem>> GetTodoItems();
        Task<TodoItem> GetTodoItem(long id);
        Task UpdateToDoItem(long id, TodoItem todoItem);
        Task AddItem(TodoItem todoItem);
        Task DeleteItem(TodoItem todoItem);
    }
}
