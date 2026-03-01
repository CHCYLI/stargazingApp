using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using StargazingApi.Data;
using StargazingApi.Services;

// run .env (more robust)
static void TryLoadDotEnv()
{
    var cwd = Directory.GetCurrentDirectory();

    var candidates = new[]
    {
        Path.Combine(cwd, ".env"),
        Path.GetFullPath(Path.Combine(cwd, "..", ".env")),
    };

    foreach (var envPath in candidates)
    {
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
            Console.WriteLine($"[env] Loaded: {envPath}");
            return;
        }
    }

    Console.WriteLine($"[env] Not found (ok): tried {string.Join(", ", candidates)}");
}

TryLoadDotEnv();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for iOS dev
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddHttpClient();

builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<ILightPollutionService, LightPollutionService>();
builder.Services.AddSingleton<IMoonService, MoonService>();
builder.Services.AddScoped<IStargazingScoreService, StargazingScoreService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("dev");

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "ok", time = DateTimeOffset.UtcNow }));

app.Run();