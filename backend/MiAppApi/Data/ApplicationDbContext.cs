using Microsoft.EntityFrameworkCore;
using MiAppApi.Models;

namespace MiAppApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Configurar timeouts
            Database.SetCommandTimeout(30);
        }

        public DbSet<Trips> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trips>(entity =>
            {
                entity.ToTable("trips");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Destination)
                    .HasColumnName("destination")
                    .HasMaxLength(255)
                    .IsRequired();
                entity.Property(e => e.DepartureDate).HasColumnName("departure_date");
                entity.Property(e => e.ReturnDate).HasColumnName("return_date");
                entity.Property(e => e.TotalSeats).HasColumnName("total_seats");
                entity.Property(e => e.SeatsAvailable).HasColumnName("seats_available");
                entity.Property(e => e.CostPerPerson)
                    .HasColumnName("cost_per_person")
                    .HasColumnType("decimal(10,2)");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Índices para mejorar rendimiento
                entity.HasIndex(e => e.DepartureDate)
                    .HasDatabaseName("IX_trips_departure_date");
                entity.HasIndex(e => e.SeatsAvailable)
                    .HasDatabaseName("IX_trips_seats_available");
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Fallback configuration - normalmente no necesario
            }
        }
    }
}