using Microsoft.EntityFrameworkCore;
using WeatherForecast.Infraestructure.Entities.DbEntities;

namespace WeatherForecast.Infraestructure.Entities.Context;

public class DistributedContext : DbContext
{
    public DistributedContext(DbContextOptions<DistributedContext> options)
    : base(options)
    {
    }

    public virtual DbSet<WeatherForecastEfEntity> WeatherForecasts { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_0900_ai_ci");

        modelBuilder.Entity<WeatherForecastEfEntity>(entity =>
        {
            entity.ToTable("WeatherForecast");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Summary).HasMaxLength(100);
        });
    }

}

