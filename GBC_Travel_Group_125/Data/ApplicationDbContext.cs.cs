
using GBC_Travel_Group_125.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Reflection.Emit;

namespace GBC_Travel_Group_125.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }


        public DbSet<Flights> Flights { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Stays> Stays { get; set; }
        public DbSet<Vehicles> Vehicles { get; set; }
        public DbSet<Events> Event { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("User");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("Role");
            });

            builder.Entity<ApplicationUser>()
            .Property(e => e.Balance)
              .HasColumnType("decimal(18, 2)"); // Precision 18, Scale 2 is commonly used

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });


        }
    }
}