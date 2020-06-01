using System;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
             // CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope())  // every time we run our app is gonna check if we got a DB if not one is gonna be created based on our migrations
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    context.Database.Migrate();
                    Seed.SeedData(context, userManager).Wait(); //wait() because we are not at an async  context 
                }
                catch (Exception ex)
                {
                   var logger = services.GetRequiredService<ILogger<Program>>();
                   logger.LogError(ex, "An error occurred during migration");
                }
            }

            host.Run();         
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
