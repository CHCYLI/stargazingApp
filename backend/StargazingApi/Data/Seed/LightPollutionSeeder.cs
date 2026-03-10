using Microsoft.EntityFrameworkCore;
using StargazingApi.Data;
using StargazingApi.Domain.Entities;
namespace StargazingApi.Data.Seed;

public static class LightPollutionSeeder
{
    public static async Task SeesAsync (AppDbContext db, string contentRootPath, CancellationToken ct = default)
    {
        var already = await db.LightPollutionGrid.AsNoTracking()
            .AnyAsync(x => x.Source == "worldatlas" && x.DataYear == 2024, ct);

        if (already)
        {
            return;
        }

        var csvPath = Path.Combine(contentRootPath, "Data", "Seed", "LightPollutionSeeder.csv");
        if (!File.Exists(csvPath))
        {
            Console.WriteLine($"[seed] light_pollution_grid.csv not found: {csvPath}");
            return;
        }

        Console.WriteLine($"[seed] Loading: {csvPath}");
    
        var lines = await File.ReadAllLinesAsync(csvPath, ct);
        if (lines.Length <= 1) return;

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(',');
            if (parts.Length < 8) continue;
        }
    }
}