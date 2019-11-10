using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PlanetHunters.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Astronomers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Astronomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StarSystems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Telescopes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    MirrorDiameter = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telescopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discoveries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateMade = table.Column<DateTime>(nullable: false),
                    TelescopeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discoveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discoveries_Telescopes_TelescopeId",
                        column: x => x.TelescopeId,
                        principalTable: "Telescopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AstronomersDiscoveries",
                columns: table => new
                {
                    AstronomerId = table.Column<int>(nullable: false),
                    DiscoveryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstronomersDiscoveries", x => new { x.AstronomerId, x.DiscoveryId });
                    table.ForeignKey(
                        name: "FK_AstronomersDiscoveries_Astronomers_AstronomerId",
                        column: x => x.AstronomerId,
                        principalTable: "Astronomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AstronomersDiscoveries_Discoveries_DiscoveryId",
                        column: x => x.DiscoveryId,
                        principalTable: "Discoveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObserversDiscoveries",
                columns: table => new
                {
                    ObserverId = table.Column<int>(nullable: false),
                    DiscoveryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObserversDiscoveries", x => new { x.ObserverId, x.DiscoveryId });
                    table.ForeignKey(
                        name: "FK_ObserversDiscoveries_Discoveries_DiscoveryId",
                        column: x => x.DiscoveryId,
                        principalTable: "Discoveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObserversDiscoveries_Astronomers_ObserverId",
                        column: x => x.ObserverId,
                        principalTable: "Astronomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Mass = table.Column<double>(nullable: false),
                    HostStarSystemId = table.Column<int>(nullable: false),
                    DiscoveryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planets_Discoveries_DiscoveryId",
                        column: x => x.DiscoveryId,
                        principalTable: "Discoveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Planets_StarSystems_HostStarSystemId",
                        column: x => x.HostStarSystemId,
                        principalTable: "StarSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Temperature = table.Column<int>(nullable: false),
                    HostStarSystemId = table.Column<int>(nullable: false),
                    DiscoveryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stars_Discoveries_DiscoveryId",
                        column: x => x.DiscoveryId,
                        principalTable: "Discoveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stars_StarSystems_HostStarSystemId",
                        column: x => x.HostStarSystemId,
                        principalTable: "StarSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AstronomersDiscoveries_DiscoveryId",
                table: "AstronomersDiscoveries",
                column: "DiscoveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Discoveries_TelescopeId",
                table: "Discoveries",
                column: "TelescopeId");

            migrationBuilder.CreateIndex(
                name: "IX_ObserversDiscoveries_DiscoveryId",
                table: "ObserversDiscoveries",
                column: "DiscoveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_DiscoveryId",
                table: "Planets",
                column: "DiscoveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_HostStarSystemId",
                table: "Planets",
                column: "HostStarSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Stars_DiscoveryId",
                table: "Stars",
                column: "DiscoveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Stars_HostStarSystemId",
                table: "Stars",
                column: "HostStarSystemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AstronomersDiscoveries");

            migrationBuilder.DropTable(
                name: "ObserversDiscoveries");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.DropTable(
                name: "Stars");

            migrationBuilder.DropTable(
                name: "Astronomers");

            migrationBuilder.DropTable(
                name: "Discoveries");

            migrationBuilder.DropTable(
                name: "StarSystems");

            migrationBuilder.DropTable(
                name: "Telescopes");
        }
    }
}
