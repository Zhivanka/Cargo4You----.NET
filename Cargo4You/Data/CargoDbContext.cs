using Microsoft.EntityFrameworkCore;
using Cargo4You.Models;

namespace Cargo4You.Data
{
    public class CargoDbContext : DbContext
    {
        public CargoDbContext(DbContextOptions<CargoDbContext> options) : base(options)
        { }

        public DbSet<UserActivityModel> UserActivities { get; set; }
    }
}
