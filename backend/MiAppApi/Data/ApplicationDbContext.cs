using Microsoft.EntityFrameworkCore;
using MiAppApi.Models;

namespace MiAppApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(30); // Opcional: timeout de comandos
        }

        public DbSet<Trips> Trips { get; set; }
        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tabla Trips
            modelBuilder.Entity<Trips>(entity =>
            {
                entity.ToTable("trips");

                entity.HasKey(e => e.Id);
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

                entity.HasIndex(e => e.DepartureDate)
                      .HasDatabaseName("IX_trips_departure_date");

                entity.HasIndex(e => e.SeatsAvailable)
                      .HasDatabaseName("IX_trips_seats_available");
            });

            // Tabla Participants
            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("participants");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TripId).HasColumnName("trip_id");

                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Phone).HasColumnName("phone");

                entity.Property(e => e.PaidAmount)
                      .HasColumnName("paid_amount")
                      .HasColumnType("decimal(10,2)")
                      .HasDefaultValue(0);

                entity.Property(e => e.PaymentComplete)
                      .HasColumnName("payment_complete")
                      .HasDefaultValue(false);

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.Trip)
                      .WithMany(t => t.Participants)
                      .HasForeignKey(e => e.TripId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Opcional: agregar configuración por defecto si se desea
                // optionsBuilder.UseSqlServer("your-connection-string");
            }
        }
    }
}
