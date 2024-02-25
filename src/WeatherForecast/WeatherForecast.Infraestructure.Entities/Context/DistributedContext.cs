using Microsoft.EntityFrameworkCore;

using WeatherForecast.Infraestructure.Entities.DbEntities;

namespace WeatherForecast.Infraestructure.Entities.Context;

public class DistributedContext(DbContextOptions<DistributedContext> options) : DbContext(options)
{
    public virtual DbSet<WeatherForecastEfEntity> WeatherForecasts { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<WeatherForecastEfEntity>(entity =>
        {
            entity.ToTable("WeatherForecast");
            entity.Property(e => e.Date).HasColumnType("datetime");
        });
    }

}

