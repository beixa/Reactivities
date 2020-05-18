using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : DbContext  //We need to add this as a service to inject it in various parts of our application, in order to query this entities in our database
    {
        public DataContext(DbContextOptions options) : base (options)
        {
            
        }

        public DbSet<Value> Values { get; set; } // a property with the table Name

        protected override void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<Value>()
                .HasData(
                    new Value {id = 1, Name = "Value 101"},
                    new Value {id = 2, Name = "Value 102"},
                    new Value {id = 3, Name = "Value 103"}
                );
        }
    }
}
