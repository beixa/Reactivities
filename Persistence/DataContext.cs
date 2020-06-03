using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser> //We need to add this as a service to inject it in various parts of our application, in order to query this entities in our database
    {
        public DataContext(DbContextOptions options) : base (options)
        {
            
        }

        public DbSet<Value> Values { get; set; } // a property with the table Name
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }

        protected override void OnModelCreating (ModelBuilder builder)
        {
            base.OnModelCreating(builder); //on creating the migration to give a AppUser a PK, otherwise error

            builder.Entity<Value>()
                .HasData(
                    new Value {id = 1, Name = "Value 101"},
                    new Value {id = 2, Name = "Value 102"},
                    new Value {id = 3, Name = "Value 103"}
                );

            builder.Entity<UserActivity>(x => x.HasKey(ua => 
                new {ua.AppUserId, ua.ActivityId}));

            builder.Entity<UserActivity>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.UserActivities)
                .HasForeignKey(u => u.AppUserId);
            
            builder.Entity<UserActivity>()
                .HasOne(a => a.Activity)
                .WithMany(u => u.UserActivities)
                .HasForeignKey(a => a.ActivityId);
        }
    }
}
