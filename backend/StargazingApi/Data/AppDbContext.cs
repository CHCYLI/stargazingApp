using Microsoft.EntityFrameworkCore;
using StargazingApi.Domain.Entities;

namespace StargazingApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<ForecastHourlyCache> ForecastHourlyCache => Set<ForecastHourlyCache>();
    public DbSet<LightPollutionGrid> LightPollutionGrid => Set<LightPollutionGrid>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ForecastHourlyCache>()
            .HasIndex(x => new { x.LatBucket, x.LonBucket, x.Time });

        modelBuilder.Entity<LightPollutionGrid>()
            .HasIndex(x => new { x.LatBucket, x.LonBucket, x.DataYear });

        modelBuilder.Entity<LightPollutionGrid>()
            .HasIndex(x => new { x.Source, x.DataYear });

        modelBuilder.Entity<LightPollutionGrid>()
            .HasIndex(x => new { x.LatBucket, x.LonBucket, x.DataYear, x.Source, x.Version })
            .IsUnique();
    }
}