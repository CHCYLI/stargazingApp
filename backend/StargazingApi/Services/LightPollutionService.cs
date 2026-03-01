using Microsoft.EntityFrameworkCore;
using StargazingApi.Common;
using StargazingApi.Data;

namespace StargazingApi.Services;

public record LightPollutionResult(
    int Bortle,
    double? BrightnessValue,
    string? BrightnessUnit,
    int DataYear,
    string Source,
    string? Version
);

public interface ILightPollutionService
{
    Task<LightPollutionResult> GetAsync(double lat, double lon, CancellationToken ct);
}

public class LightPollutionService : ILightPollutionService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _cfg;

    public LightPollutionService(AppDbContext db, IConfiguration cfg)
    {
        _db = db;
        _cfg = cfg;
    }

    public async Task<LightPollutionResult> GetAsync(double lat, double lon, CancellationToken ct)
    {
        var (latB, lonB) = GeoBucket.Round2(lat, lon);

        var preferredSource = _cfg.GetValue<string?>("LIGHTPOLLUTION_PREFERRED_SOURCE");
        var preferredYear = _cfg.GetValue<int?>("LIGHTPOLLUTION_PREFERRED_YEAR");

        var q = _db.LightPollutionGrid.AsNoTracking()
            .Where(x => x.LatBucket == latB && x.LonBucket == lonB);

        if (!string.IsNullOrWhiteSpace(preferredSource))
            q = q.Where(x => x.Source == preferredSource);

        if (preferredYear is not null)
            q = q.Where(x => x.DataYear == preferredYear.Value);

        var hit = await q.OrderByDescending(x => x.DataYear)
                         .ThenByDescending(x => x.Version)
                         .FirstOrDefaultAsync(ct);

        if (hit is null)
        {
            // MVP：查不到照旧给默认 7
            return new LightPollutionResult(
                Bortle: 7,
                BrightnessValue: null,
                BrightnessUnit: null,
                DataYear: preferredYear ?? 2024,
                Source: preferredSource ?? "default",
                Version: null
            );
        }

        var bortle = (hit.Bortle is >= 1 and <= 9) ? hit.Bortle : 7;

        return new LightPollutionResult(
            bortle,
            hit.BrightnessValue,
            hit.BrightnessUnit,
            hit.DataYear,
            hit.Source,
            hit.Version
        );
    }
}