using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StargazingApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Lat = table.Column<double>(type: "REAL", nullable: false),
                    Lon = table.Column<double>(type: "REAL", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForecastHourlyCache",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LatBucket = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    LonBucket = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    Time = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CloudCover = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecipProb = table.Column<int>(type: "INTEGER", nullable: false),
                    WindSpeed = table.Column<double>(type: "REAL", nullable: false),
                    Humidity = table.Column<int>(type: "INTEGER", nullable: false),
                    FetchedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForecastHourlyCache", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LightPollutionGrid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LatBucket = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    LonBucket = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    Bortle = table.Column<int>(type: "INTEGER", nullable: false),
                    BrightnessValue = table.Column<double>(type: "REAL", nullable: true),
                    BrightnessUnit = table.Column<string>(type: "TEXT", maxLength: 24, nullable: true),
                    DataYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Source = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Version = table.Column<string>(type: "TEXT", maxLength: 24, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightPollutionGrid", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForecastHourlyCache_LatBucket_LonBucket_Time",
                table: "ForecastHourlyCache",
                columns: new[] { "LatBucket", "LonBucket", "Time" });

            migrationBuilder.CreateIndex(
                name: "IX_LightPollutionGrid_LatBucket_LonBucket_DataYear",
                table: "LightPollutionGrid",
                columns: new[] { "LatBucket", "LonBucket", "DataYear" });

            migrationBuilder.CreateIndex(
                name: "IX_LightPollutionGrid_LatBucket_LonBucket_DataYear_Source_Version",
                table: "LightPollutionGrid",
                columns: new[] { "LatBucket", "LonBucket", "DataYear", "Source", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LightPollutionGrid_Source_DataYear",
                table: "LightPollutionGrid",
                columns: new[] { "Source", "DataYear" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "ForecastHourlyCache");

            migrationBuilder.DropTable(
                name: "LightPollutionGrid");
        }
    }
}
