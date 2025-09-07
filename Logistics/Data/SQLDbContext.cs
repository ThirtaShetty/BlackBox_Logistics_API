using Microsoft.EntityFrameworkCore;
using Logistics.Models;

namespace Logistics.Data
{
    public class SQLDbContext : DbContext
    {
        public SQLDbContext(DbContextOptions<SQLDbContext> options): base(options){ }

        // Example DbSet properties
        public DbSet<Load> Load { get; set; }
        public DbSet<Truck> Truck { get; set; }
        public DbSet<LoadsRoute> LoadsRoute { get; set; }
        public DbSet<Hubspot> Hubspot { get; set; }

    }
}