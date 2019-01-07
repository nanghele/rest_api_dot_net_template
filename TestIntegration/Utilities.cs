using System;
using MyAPI.Models;

namespace TestIntegration
{
    internal class Utilities
    {
        internal static void InitializeDbForTests(TodoContext db)
        {
            db.Add(new TodoItem { Name = "Item1", IsComplete=true });
            db.Add(new TodoItem { Name = "Item2", IsComplete = false });
            db.Add(new TodoItem { Name = "Item3", IsComplete = false });
            db.Add(new TodoItem { Name = "Item4", IsComplete = true });
            db.SaveChanges();
        }
    }
}