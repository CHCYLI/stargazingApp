# Stargazing App

Stargazing App is a .NET 8 backend API for calculating stargazing conditions by combining:

- weather forecast data (Open-Meteo),
- moon illumination,
- light pollution metadata,
- and a simple scoring model.

The repo currently contains a backend API and an `ios/` directory placeholder.

## Tech Stack

- .NET SDK `8.0.418` (see `global.json`)
- ASP.NET Core Web API
- Entity Framework Core + SQLite
- Swagger/OpenAPI

## Repository Layout

```text
.
├─ backend/
│  ├─ StargazingApi/               # ASP.NET Core API
│  ├─ .env.example                 # local env template
│  ├─ run.ps1                      # Windows run script
│  └─ run.sh                       # macOS/Linux run script
├─ ios/                            # currently empty placeholder
├─ stargazingApp.sln
└─ global.json
```

## Quick Start

### 1. Prerequisites

- .NET SDK 8 installed

Check:

```bash
dotnet --version
```

### 2. Configure environment

From the `backend/` directory, create `.env` from the example:

```bash
cp .env.example .env
```

Windows PowerShell alternative:

```powershell
Copy-Item .env.example .env
```

### 3. Restore and run

Option A (recommended scripts):

```powershell
# Windows
./run.ps1
```

```bash
# macOS/Linux
./run.sh
```

Option B (manual):

```bash
cd backend/StargazingApi
dotnet tool restore
dotnet restore
dotnet run
```

The API runs at:

- `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`
- Health check: `http://localhost:5000/health`

## Environment Variables

The backend reads `.env` from either:

- `backend/StargazingApi/.env`, or
- `backend/.env`

Current variables in `backend/.env.example`:

- `ASPNETCORE_ENVIRONMENT=Development`
- `ASPNETCORE_URLS=http://localhost:5000`
- `CONNECTIONSTRINGS__DEFAULT=Data Source=stargaze.db`
- `WEATHER_PROVIDER=open-meteo`
- `WEATHER_CACHE_TTL_MINUTES=120`

Optional light pollution selection (used by `LightPollutionService`):

- `LIGHTPOLLUTION_PREFERRED_SOURCE`
- `LIGHTPOLLUTION_PREFERRED_YEAR`

## API Overview

### `GET /api/score`

Returns a stargazing score forecast for a location.

Query params:

- `lat` (required, -90..90)
- `lon` (required, -180..180)
- `hours` (optional, clamped to 6..48, default 24)

Example:

```bash
curl "http://localhost:5000/api/score?lat=40.7128&lon=-74.0060&hours=24"
```

### Favorites

- `GET /api/favorites`
- `POST /api/favorites`
- `DELETE /api/favorites/{id}`

Create example:

```bash
curl -X POST "http://localhost:5000/api/favorites" \
  -H "Content-Type: application/json" \
  -d "{\"name\":\"Backyard\",\"lat\":40.71,\"lon\":-74.00}"
```

## Database and Migrations

This project uses EF Core with SQLite and includes an initial migration.

Typical migration commands (from `backend/StargazingApi`):

```bash
dotnet ef database update
```

```bash
dotnet ef migrations add <MigrationName>
```

## Testing

Run tests from repo root:

```bash
dotnet test
```

Note: there is currently a test project scaffold, but no implemented test cases yet.

## Current Status Notes

- `ios/` exists but is currently empty.
- A light-pollution seeder file exists but is not yet wired into startup flow.

