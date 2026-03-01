using System.ComponentModel.DataAnnotations;

namespace StargazingApi.Domain.Entities;

public class LightPollutionGrid
{
    public int Id { get; set; }

    [MaxLength(16)]
    public string LatBucket { get; set; } = default!;

    [MaxLength(16)]
    public string LonBucket { get; set; } = default!;

    public int Bortle { get; set; } = 7; // 1-9

    public double? BrightnessValue { get; set; }

    // e.g. "mcd_m2", "mag_arcsec2", "radiance_nw_cm2_sr"
    [MaxLength(24)]
    public string? BrightnessUnit { get; set; }

    // e.g. 2024
    public int DataYear { get; set; } = 2024;

    [MaxLength(40)]
    public string Source { get; set; } = "manual";  // e.g. "worldatlas", "eog_viirs", "manual"

    [MaxLength(24)]
    public string? Version { get; set; } // e.g. "v2024.1"

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}