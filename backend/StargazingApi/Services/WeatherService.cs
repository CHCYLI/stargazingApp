using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StargazingApi.Common;
using StargazingApi.Data;
using StargazingApi.Domain.Entities;

namespace StargazingApi.Services;

public record WeatherHour(DateTimeOffset Time, int CloudCover, int PrecipProb, double WindSpeed, int Humidity);

public interface IWeatherService
{
    Task<List<WeatherHour>> GetHourlyAsync(double lat, double lon, int hours, CancellationToken ct);
}

public class WeatherService : IWeatherService
{
    private readonly AppDbContext _db;
    private readonly HttpClient _http;
    private readonly TimeSpan _ttl;

    public WeatherService(AppDbContext db, HttpClient http, IConfiguration cfg)
    {
        _db = db;
        _http = http;

        // .env: WEATHER_CACHE_TTL_MINUTES=120
        var ttlMin = cfg.GetValue<int?>("WEATHER_CACHE_TTL_MINUTES") ?? 120;
        _ttl = TimeSpan.FromMinutes(Math.Clamp(ttlMin, 10, 720));
    }

    public async Task<List<WeatherHour>> GetHourlyAsync(double lat, double lon, int hours, CancellationToken ct)
    {
        hours = Math.Clamp(hours, 6, 48);

        var (latB, lonB) = GeoBucket.Round2(lat, lon);
        var now = DateTimeOffset.UtcNow;
        var start = GeoBucket.FloorToHourUtc(now);
        var end = start.AddHours(hours);

        // 1) Try cache
        var cached = await _db.ForecastHourlyCache
            .Where(x => x.LatBucket == latB && x.LonBucket == lonB && x.Time >= start && x.Time < end)
            .OrderBy(x => x.Time)
            .ToListAsync(ct);

        var fresh = cached.Count == hours && cached.All(x => (now - x.FetchedAt) <= _ttl);

        if (fresh)
        {
            return cached.Select(x => new WeatherHour(x.Time, x.CloudCover, x.PrecipProb, x.WindSpeed, x.Humidity)).ToList();
        }

        // 2) Fetch Open-Meteo
        var url =
            "https://api.open-meteo.com/v1/forecast" +
            $"?latitude={lat}&longitude={lon}" +
            "&hourly=cloudcover,precipitation_probability,windspeed_10m,relativehumidity_2m" +
            "&timezone=UTC";

        using var resp = await _http.GetAsync(url, ct);
        if (!resp.IsSuccessStatusCode)
            throw new HttpRequestException($"Open-Meteo failed: {(int)resp.StatusCode}");

        using var stream = await resp.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

        var hourly = doc.RootElement.GetProperty("hourly");

        var times = hourly.GetProperty("time").EnumerateArray().Select(e => DateTimeOffset.Parse(e.GetString()!)).ToList();
        var cloud = hourly.GetProperty("cloudcover").EnumerateArray().Select(e => e.GetInt32()).ToList();
        var precip = hourly.GetProperty("precipitation_probability").EnumerateArray().Select(e => e.GetInt32()).ToList();
        var wind = hourly.GetProperty("windspeed_10m").EnumerateArray().Select(e => e.GetDouble()).ToList();
        var hum = hourly.GetProperty("relativehumidity_2m").EnumerateArray().Select(e => e.GetInt32()).ToList();

        var selected = new List<WeatherHour>();
        for (int i = 0; i < times.Count; i++)
        {
            if (times[i] < start || times[i] >= end) continue;
            selected.Add(new WeatherHour(times[i], cloud[i], precip[i], wind[i], hum[i]));
        }

        // 3) Upsert cache segment
        var old = await _db.ForecastHourlyCache
            .Where(x => x.LatBucket == latB && x.LonBucket == lonB && x.Time >= start && x.Time < end)
            .ToListAsync(ct);

        _db.ForecastHourlyCache.RemoveRange(old);

        foreach (var h in selected)
        {
            _db.ForecastHourlyCache.Add(new ForecastHourlyCache
            {
                LatBucket = latB,
                LonBucket = lonB,
                Time = h.Time,
                CloudCover = h.CloudCover,
                PrecipProb = h.PrecipProb,
                WindSpeed = h.WindSpeed,
                Humidity = h.Humidity,
                FetchedAt = now
            });
        }

        await _db.SaveChangesAsync(ct);
        return selected;
    }
}