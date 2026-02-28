using System.ComponentModel.DataAnnotations;
namespace StargazingApi.Domain.Entities;
public class Favorite
{
    public int Id
    {
        get;
        set;
    }
    [MaxLength(80)]
    public string Name
    {
        get;
        set;
    } = "Favorite";
    public double Lat {
        get;
        set;
    }
    public double Lon {
        get;
        set;
    }
    public DateTimeOffset CreatedAt {
        get;
        set;
    } = DateTimeOffset.UtcNow;
}