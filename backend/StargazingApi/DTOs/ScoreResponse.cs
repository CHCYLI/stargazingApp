namespace StargazingApi.DTOs;

public record ScoreResponse(
    LocationDto Location,
    DateTimeOffset GeneratedAt,
    SummaryDto Summary,
    List<BreakdownItemDto> BreakdownNow,
    List<HourlyScoreDto> Hourly,
    List<BestWindowDto> BestWindows
);

public record LocationDto(double Lat, double Lon, string Name);

public record LightPollutionMetaDto(
    int Bortle,
    double? Value,
    string? Unit,
    int DataYear,
    string Source,
    string? Version
);

public record SummaryDto(
    int ScoreNow,
    string Rating,
    int Bortle, // 保留旧字段（兼容）
    int MoonIllumination,
    LightPollutionMetaDto LightPollution
);

public record BreakdownItemDto(string Factor, double Value, double Penalty, string Note);

public record HourlyScoreDto(
    DateTimeOffset Time,
    int CloudCover,
    int PrecipProb,
    double WindSpeed,
    int Humidity,
    int MoonIllumination,
    int Bortle,
    int Score
);

public record BestWindowDto(DateTimeOffset Start, DateTimeOffset End, int AvgScore, string Reason);