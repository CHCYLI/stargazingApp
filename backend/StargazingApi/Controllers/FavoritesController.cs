using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StargazingApi.Data;
using StargazingApi.Domain.Entities;
using StargazingApi.DTOs;

namespace StargazingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly AppDbContext _db;
    public FavoritesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<FavoriteDto>>> Get(CancellationToken ct)
    {
        var rows = await _db.Favorites.AsNoTracking().ToListAsync(ct);

        var res = rows
            .OrderByDescending(x => x.CreatedAt) // ✅ in-memory sort (SQLite + DateTimeOffset safer)
            .Select(x => new FavoriteDto(x.Id, x.Name, x.Lat, x.Lon, x.CreatedAt))
            .ToList();

        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<FavoriteDto>> Create([FromBody] CreateFavoriteRequest req, CancellationToken ct)
    {
        if (req.Lat is < -90 or > 90) return BadRequest("lat out of range");
        if (req.Lon is < -180 or > 180) return BadRequest("lon out of range");
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest("name required");

        var f = new Favorite { Name = req.Name.Trim(), Lat = req.Lat, Lon = req.Lon };
        _db.Favorites.Add(f);
        await _db.SaveChangesAsync(ct);

        return Ok(new FavoriteDto(f.Id, f.Name, f.Lat, f.Lon, f.CreatedAt));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var f = await _db.Favorites.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (f is null) return NotFound();

        _db.Favorites.Remove(f);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}