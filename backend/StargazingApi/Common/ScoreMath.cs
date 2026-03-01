namespace StargazingApi.Common;

public static class ScoreMath
{
    public static int ClampScore(double x) => (int)Math.Clamp(Math.Round(x), 0, 100);

    public static double WindPenalty(double windKmh)
    {
        // Open-Meteo windspeed_10m - default unit: km/h
        if (windKmh <= 15) return 0;  // ~9 mph
        if (windKmh <= 30) return (windKmh - 15) * 0.6;
        return 9 + (windKmh - 30) * 1.0;
    }

    public static double HumidityPenalty(int humidity)
    {
        if (humidity <= 70) return 0;
        if (humidity <= 85) return (humidity - 70) * 0.4;
        return 6 + (humidity - 85) * 0.8;
    }

    public static double LightPollutionPenalty(int bortle) => bortle switch
    {
        1 => 0, 2 => 3, 3 => 7, 4 => 12, 5 => 18,
        6 => 24, 7 => 30, 8 => 38, 9 => 45,
        _ => 30
    };

    public static string Rating(int score) => score switch
    {
        >= 80 => "Great",
        >= 60 => "OK",
        _ => "Poor"
    };
}