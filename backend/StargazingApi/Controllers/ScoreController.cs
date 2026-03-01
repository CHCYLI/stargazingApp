using Microsoft.AspNetCore.Mvc;
using StargazingApi.Services;

namespace StargazingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoreController : ControllerBase
{
    private readonly IStargazingScoreService _svc;
    private readonly ILogger<ScoreController> _log;

    public ScoreController(IStargazingScoreService svc, ILogger<ScoreController> log)
    {
        _svc = svc;
        _log = log;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] double lat, [FromQuery] double lon, [FromQuery] int hours = 24, CancellationToken ct = default)
    {
        if (lat is < -90 or > 90) return BadRequest("lat out of range");
        if (lon is < -180 or > 180) return BadRequest("lon out of range");

        try
        {
            var res = await _svc.GetScoreAsync(lat, lon, hours, ct);
            return Ok(res);
        }
        catch (HttpRequestException ex)
        {
            _log.LogWarning(ex, "External weather API failed");
            return Problem(
                title: "Weather provider unavailable",
                detail: ex.Message,
                statusCode: StatusCodes.Status503ServiceUnavailable
            );
        }
    }
}