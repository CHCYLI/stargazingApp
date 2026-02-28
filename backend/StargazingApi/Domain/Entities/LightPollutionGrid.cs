using System.ComponentModel.DataAnnotations;

namespace StargazingApi.Domain.Entities;

public class LightPollutionGrid
{
    public int Id { get; set; }

    [MaxLength(16)]
    public string LatBucket { get; set; } = default!;

    [MaxLength(16)]
    public string LonBucket { get; set; } = default!;

    public int Bortle { get; set; } // 1-9, higher the ligher, vice versa

    [MaxLength(40)]
    public string Source { get; set; } = "manual";

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}