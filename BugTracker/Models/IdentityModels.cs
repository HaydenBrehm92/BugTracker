using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ICollection<Project> Projects { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<BugProperties> BugProperties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BugProperties>()
                .HasRequired(s => s.Project)
                .WithMany(p => p.GetBugs)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(s => s.Projects)
                .WithRequired()
                .WillCascadeOnDelete(true);


            //modelBuilder.Entity<Project>()
            //    .HasRequired(s => s.ApplicationUserID)
            //    .WithMany()
            //    .HasForeignKey(a => a.ApplicationUserID)
            //    .WillCascadeOnDelete(true);


            //modelBuilder.Entity<IdentityUser>()
            //    .ha(a => a.)
            //    .WithMany(b => b.GetBugs)
            //    .

            //modelBuilder.Entity<Project>()
            //    .HasMany(b => b.GetBugs)
            //    .WithRequired(s => s.Project)
            //    .WillCascadeOnDelete(true);
        }
    }
}