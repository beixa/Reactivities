using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser> //We need to add this as a service to inject it in various parts of our application, in order to query this entities in our database
    {
        public DataContext(DbContextOptions options) : base (options){} // with the constructor we make sure we are resolving the options configured in the dependency injection container (addDbContext()) and passing them to de base

        public DbSet<Value> Values { get; set; } // a property with the table Name
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFollowing> Followings { get; set; }//we need a dbset because we want a direct access to the followings(context) and not via navigation property

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

            builder.Entity<UserFollowing>(b =>
            {
                b.HasKey(k => new {k.ObserverId, k.TargetId});

                b.HasOne(o => o.Observer)
                    .WithMany(f =>f.Followings)
                    .HasForeignKey(o => o.ObserverId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(o => o.Target)
                    .WithMany(f =>f.Followers)
                    .HasForeignKey(o => o.TargetId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
