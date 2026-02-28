using DotNetEnv;
//run .env
static void TryLoadDotEnv()
{
   
    var envPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", ".env"));
    if (File.Exists(envPath))
    {
        Env.Load(envPath);
        Console.WriteLine($"[env] Loaded: {envPath}");
    }
    else
    {
        Console.WriteLine($"[env] Not found (ok): {envPath}");
    }
}

TryLoadDotEnv();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("dev");

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "ok", time = DateTimeOffset.UtcNow }));

app.Run();