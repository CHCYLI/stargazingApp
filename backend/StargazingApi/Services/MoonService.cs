namespace StargazingApi.Services;

public interface IMoonService
{
    int GetMoonIlluminationPercent(DateTimeOffset timeUtc);
}

public class MoonService : IMoonService
{
    public int GetMoonIlluminationPercent(DateTimeOffset timeUtc)
    {
        return 48;
    }
}