using Microsoft.EntityFrameworkCore;
using IncidentsService.Models;

namespace IncidentsService.Data
{
    public class IncidentsDbContext : DbContext
    {
        public IncidentsDbContext(DbContextOptions<IncidentsDbContext> options) : base(options) { }
        
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentVote> IncidentVotes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the table names with explicit casing
            modelBuilder.Entity<Incident>().ToTable("Incidents");
            modelBuilder.Entity<IncidentVote>().ToTable("IncidentVotes");
        }
    }
}