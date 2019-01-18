using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using MyAPI.Services;
using MyAPI.Filters;

namespace TestIntegration
{
    public class CustomWebApplicationFactory<TStartup>
     : WebApplicationFactory<MyAPI.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<MyAPI.Models.TodoContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(serviceProvider);
                });
                // This object will be available in DI container
                services.AddScoped<IToDoService, ToDoService>();
                
                // Managing exception in a centralized way
                services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(CustomExceptionFilter));
                    
                });

                
              

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<MyAPI.Models.TodoContext>();               
                    // Ensure the database is created.
                    db.Database.EnsureCreated();
                    try
                    {
                        // Seed the database with test data.
                        Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine( "An error occurred seeding the " +
                            "database with test messages. Error: "+ex.Message);
                    }
                }
            });
        }
    }
}
