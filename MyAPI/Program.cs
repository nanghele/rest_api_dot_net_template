using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace MyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IConfigurationRoot GetConfig() {
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{currentEnv}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
    }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {

            var webhostbuilder = WebHost.CreateDefaultBuilder(args);
            
                webhostbuilder.UseSerilog((ctx, config) =>
                {
                    config.ReadFrom.Configuration(GetConfig())
                        .WriteTo.Console();
                });      
            return webhostbuilder.UseStartup<Startup>();
        }
    }
}
