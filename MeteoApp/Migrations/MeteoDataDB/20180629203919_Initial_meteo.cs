using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MeteoApp.Migrations.MeteoDataDB
{
    public partial class Initial_meteo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    ChangedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DaysData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    ChangedOn = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Precipitation = table.Column<decimal>(nullable: false),
                    StationId = table.Column<int>(nullable: false),
                    Temperature = table.Column<decimal>(nullable: false),
                    ThunderCount = table.Column<int>(nullable: false),
                    Wind = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaysData_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StationsAvailabilityPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    ChangedOn = table.Column<DateTime>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    StationId = table.Column<int>(nullable: false),
                    To = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationsAvailabilityPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StationsAvailabilityPeriods_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StationsWeights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    ChangedOn = table.Column<DateTime>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    StationId = table.Column<int>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationsWeights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StationsWeights_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DaysData_StationId",
                table: "DaysData",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_StationsAvailabilityPeriods_StationId",
                table: "StationsAvailabilityPeriods",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_StationsWeights_StationId",
                table: "StationsWeights",
                column: "StationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaysData");

            migrationBuilder.DropTable(
                name: "StationsAvailabilityPeriods");

            migrationBuilder.DropTable(
                name: "StationsWeights");

            migrationBuilder.DropTable(
                name: "Stations");
        }
    }
}
