using Infraestructure.MySqlDatabase.DatabaseEntities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.MySqlDatabase.Contexts
{
    public partial class CleanArchitectureSkeletonContext : DbContext
    {
        public CleanArchitectureSkeletonContext(DbContextOptions<CleanArchitectureSkeletonContext> options)
            : base(options)
        {
        }

        public virtual DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<WeatherForecast>(entity =>
            {
                entity.ToTable("WeatherForecast");
                entity.Property(e => e.Date).HasColumnType("datetime");
                entity.Property(e => e.Summary).HasMaxLength(100);
            });
        }
    }
}
