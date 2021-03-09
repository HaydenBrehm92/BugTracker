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
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            bool adminResult = roleManager.RoleExists("Admin");
            if(!adminResult)
            {
                roleManager.Create(new IdentityRole("Admin"));
            }

            bool userResult = roleManager.RoleExists("User");
            if(!userResult)
            {
                roleManager.Create(new IdentityRole("User"));
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var adminUser = userManager.FindByName("haydenbrehm92@gmail.com");
            if(adminUser == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "haydenbrehm92@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PasswordHash = password,
                    Email = "haydenbrehm92@gmail.com",
                    LockoutEnabled = true,
                    EmailConfirmed = true
                };
                var createResult = userManager.Create(newUser);
                if (createResult.Succeeded)
                {
                    userManager.AddToRole(newUser.Id, "Admin");
                }
            }
            
            if(!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }

            //if(!context.Roles.Any(r => r.Name == "Admin"))
            //{
            //    //var store = new RoleStore<IdentityRole>(context);
            //    //var manager = new RoleManager<IdentityRole>(store);
            //    var role = new IdentityRole { Id = "1", Name = "Admin" };

            //    manager.Create(role);
            //}

            //if(!context.Roles.Any(r => r.Name == "User"))
            //{
            //    var store = new RoleStore<IdentityRole>(context);
            //    var manager = new RoleManager<IdentityRole>(store);
            //    var role = new IdentityRole { Id = "2", Name = "User" };
            //    manager.Create(role);
            //}

            //if(!context.Users.Any(u => u.UserName == "haydenbrehm92@gmail.com"))
            //{
            //    var store = new UserStore<ApplicationUser>(context);
            //    var manager = new UserManager<ApplicationUser>(store);
            //    var user = new ApplicationUser
            //    {
            //        UserName = "haydenbrehm92@gmail.com",
            //        SecurityStamp = Guid.NewGuid().ToString(),
            //        PasswordHash = password,
            //        Email = "haydenbrehm92@gmail.com",
            //        LockoutEnabled = true,
            //        EmailConfirmed = true
            //    };

            //    manager.Create(user);
            //    manager.AddToRole(user.Id, "Admin");
            //}
        }
    }
}
