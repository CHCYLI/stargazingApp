namespace StargazingApi.Common;
//coordinate
public static class GeoBucket
{
    public static (string latBucket, string lonBucket) Round2(double lat, double lon)
    {
        var lb = Math.Round(lat, 2, MidpointRounding.AwayFromZero).ToString("F2");
        var ob = Math.Round(lon, 2, MidpointRounding.AwayFromZero).ToString("F2");
        return (lb, ob);
    }

    public static DateTimeOffset FloorToHourUtc(DateTimeOffset utc)
        => new DateTimeOffset(utc.Year, utc.Month, utc.Day, utc.Hour, 0, 0, TimeSpan.Zero);
}