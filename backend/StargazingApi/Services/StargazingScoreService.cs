using StargazingApi.Common;
using StargazingApi.DTOs;

namespace StargazingApi.Services;

public interface IStargazingScoreService
{
    Task<ScoreResponse> GetScoreAsync(double lat, double lon, int hours, CancellationToken ct);
}

public class StargazingScoreService : IStargazingScoreService
{
    private readonly IWeatherService _weather;
    private readonly ILightPollutionService _light;
    private readonly IMoonService _moon;

    public StargazingScoreService(IWeatherService weather, ILightPollutionService light, IMoonService moon)
    {
        _weather = weather;
        _light = light;
        _moon = moon;
    }

    public async Task<ScoreResponse> GetScoreAsync(double lat, double lon, int hours, CancellationToken ct)
    {
        hours = Math.Clamp(hours, 6, 48);

        var lp = await _light.GetAsync(lat, lon, ct);
        var bortle = lp.Bortle;

        var wx = await _weather.GetHourlyAsync(lat, lon, hours, ct);

        var hourly = new List<HourlyScoreDto>(wx.Count);

        foreach (var h in wx)
        {
            var moonIll = _moon.GetMoonIlluminationPercent(h.Time);

            double score = 100;
            score -= h.CloudCover * 0.6;
            score -= h.PrecipProb * 0.3;
            score -= ScoreMath.WindPenalty(h.WindSpeed);
            score -= ScoreMath.HumidityPenalty(h.Humidity);
            score -= moonIll * 0.2;
            score -= ScoreMath.LightPollutionPenalty(bortle);

            hourly.Add(new HourlyScoreDto(
                h.Time,
                h.CloudCover,
                h.PrecipProb,
                h.WindSpeed,
                h.Humidity,
                moonIll,
                bortle,
                ScoreMath.ClampScore(score)
            ));
        }

        var now = hourly.FirstOrDefault();
        var scoreNow = now?.Score ?? 0;
        var moonNow = now?.MoonIllumination ?? _moon.GetMoonIlluminationPercent(DateTimeOffset.UtcNow);

        var breakdownNow = BuildBreakdown(now, bortle, moonNow);
        var bestWindows = FindBestWindows(hourly, minLengthHours: 2);

        var lpMeta = new LightPollutionMetaDto(
            Bortle: lp.Bortle,
            Value: lp.BrightnessValue,
            Unit: lp.BrightnessUnit,
            DataYear: lp.DataYear,
            Source: lp.Source,
            Version: lp.Version
        );

        return new ScoreResponse(
            new LocationDto(lat, lon, "Pinned Location"),
            DateTimeOffset.UtcNow,
            new SummaryDto(scoreNow, ScoreMath.Rating(scoreNow), bortle, moonNow, lpMeta),
            breakdownNow,
            hourly,
            bestWindows
        );
    }

    private static List<BreakdownItemDto> BuildBreakdown(HourlyScoreDto? now, int bortle, int moonIll)
    {
        if (now is null) return new();

        var items = new List<BreakdownItemDto>
        {
            new("cloud_cover", now.CloudCover, now.CloudCover * 0.6, now.CloudCover >= 70 ? "Cloudy sky" : "Moderate clouds"),
            new("rain", now.PrecipProb, now.PrecipProb * 0.3, now.PrecipProb >= 50 ? "High rain chance" : "Low rain chance"),
            new("wind", now.WindSpeed, ScoreMath.WindPenalty(now.WindSpeed), "Wind reduces stability"),
            new("humidity", now.Humidity, ScoreMath.HumidityPenalty(now.Humidity), "High humidity reduces clarity"),
            new("moon", moonIll, moonIll * 0.2, "Moonlight reduces contrast"),
            new("light_pollution", bortle, ScoreMath.LightPollutionPenalty(bortle), "Urban sky glow")
        };

        return items.OrderByDescending(x => x.Penalty).ToList();
    }

    private static List<BestWindowDto> FindBestWindows(List<HourlyScoreDto> hourly, int minLengthHours)
    {
        if (hourly.Count == 0) return new();

        BestWindowDto? best = null;

        for (int i = 0; i < hourly.Count; i++)
        {
            int sum = 0;
            for (int j = i; j < hourly.Count; j++)
            {
                sum += hourly[j].Score;
                var len = j - i + 1;
                if (len < minLengthHours) continue;

                var avg = sum / len;
                var cand = new BestWindowDto(
                    hourly[i].Time,
                    hourly[j].Time.AddHours(1),
                    avg,
                    "Highest average score in this range"
                );

                if (best is null || cand.AvgScore > best.AvgScore)
                    best = cand;
            }
        }

        return best is null ? new() : new List<BestWindowDto> { best };
    }
}