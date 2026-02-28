using System.ComponentModel.DataAnnotations;
namespace StargazingApi.Domain.Entities;
public class ForecastHourlyCache
{
    public int ID {get;set;}
    [MaxLength(16)]
    public string LatBucket { get; set; } = default!;

    [MaxLength(16)]
    public string LonBucket { get; set; } = default!;
    public DateTimeOffset Time { get; set; }  
    //1-100 representation
    public int CloudCover { get; set; }   
    public int PrecipProb { get; set; }    
    public double WindSpeed { get; set; }     
    public int Humidity { get; set; }   
    public DateTimeOffset FetchedAt { get; set; } = DateTimeOffset.UtcNow;
}