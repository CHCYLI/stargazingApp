namespace StargazingApi.DTOs;

public record CreateFavoriteRequest(string Name, double Lat, double Lon);
public record FavoriteDto(int Id, string Name, double Lat, double Lon, DateTimeOffset CreatedAt);