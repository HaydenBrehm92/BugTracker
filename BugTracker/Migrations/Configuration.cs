namespace BugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using BugTracker.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true; //was true
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var pwHash = new PasswordHasher();
            string password = pwHash.HashPassword("Gamegrumps5!");

            if(!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Id = "1", Name = "Admin" };

                manager.Create(role);
            }

            if(!context.Roles.Any(r => r.Name == "User"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Id = "2", Name = "User" };
                manager.Create(role);
            }

            if(!context.Users.Any(u => u.UserName == "haydenbrehm92@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "haydenbrehm92@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PasswordHash = password,
                    Email = "haydenbrehm92@gmail.com",
                    LockoutEnabled = true,
                    EmailConfirmed = true
                };

                manager.Create(user);
                manager.AddToRole(user.Id, "Admin");
            }



            //var user = new IdentityUser
            //{
            //    UserName = "haydenbrehm92@gmail.com",
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //    PasswordHash = password,
            //    Email = "haydenbrehm92@gmail.com",
            //    LockoutEnabled = true

            //};

            //var rolechoice = new IdentityRole("Admin");


            //context.Users.AddOrUpdate(u => u.UserName, new ApplicationUser
            //{
            
            
            //    UserName = "haydenbrehm92@gmail.com",
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //    PasswordHash = password,
            //    Email = "haydenbrehm92@gmail.com",
            //    LockoutEnabled = true
            
            
            //});
            
            //context.Roles.AddOrUpdate(new IdentityRole
            //{
            //    Id = "1",
            //    Name = "Admin"
            //});
            //context.SaveChanges();

            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //manager.AddToRole(user.Id, "Admin");
            
            
        }
    }
}
