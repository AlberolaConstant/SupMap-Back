using Microsoft.EntityFrameworkCore;
using IncidentsService.Models;

namespace IncidentsService.Data
{
    public class IncidentsDbContext : DbContext
    {
        public IncidentsDbContext(DbContextOptions<IncidentsDbContext> options)
            : base(options) { }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentVote> IncidentVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Incident>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<IncidentVote>()
                .HasKey(v => v.Id);

            modelBuilder.Entity<IncidentVote>()
                .HasOne(v => v.Incident)
                .WithMany(i => i.Votes)
                .HasForeignKey(v => v.IncidentId);

            // Ajouter un index composite pour empêcher les votes multiples du même utilisateur
            modelBuilder.Entity<IncidentVote>()
                .HasIndex(v => new { v.IncidentId, v.UserId })
                .IsUnique();
        }
    }
}