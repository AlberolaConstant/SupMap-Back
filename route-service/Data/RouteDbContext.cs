using Microsoft.EntityFrameworkCore;
using RoutesService.Models;

namespace RoutesService.Data
{
    public class RoutesDbContext : DbContext
    {
        public RoutesDbContext(DbContextOptions<RoutesDbContext> options)
            : base(options) { }

        public DbSet<RoutesService.Models.Road> Routes { get; set; }
    }
}
