using Microsoft.EntityFrameworkCore;
using MiAppApi.Models;

namespace MiAppApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trips> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trips>(entity =>
            {
                entity.ToTable("trips");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Destination).HasColumnName("destination");
                entity.Property(e => e.DepartureDate).HasColumnName("departure_date");
                entity.Property(e => e.ReturnDate).HasColumnName("return_date");
                entity.Property(e => e.TotalSeats).HasColumnName("total_seats");
                entity.Property(e => e.SeatsAvailable).HasColumnName("seats_available");
                entity.Property(e => e.CostPerPerson).HasColumnName("cost_per_person");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
