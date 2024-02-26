using GBC_Travel_Group_125.Models;
using Microsoft.EntityFrameworkCore;

namespace GBC_Travel_Group_125.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
        
        }

  
        public DbSet<Flights> Flights { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Stays> Stays { get; set; }
        public DbSet<Vehicles> Vehicles { get; set; }
        public DbSet<Events> Event { get; set; }
      


    }
}
