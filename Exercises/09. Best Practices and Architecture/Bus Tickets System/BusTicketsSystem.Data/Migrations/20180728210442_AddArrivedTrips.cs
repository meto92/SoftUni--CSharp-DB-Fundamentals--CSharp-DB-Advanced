using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusTicketsSystem.Data.Migrations
{
    public partial class AddArrivedTrips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArrivedTrips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    PassengersCount = table.Column<int>(nullable: false),
                    OriginBusStationId = table.Column<int>(nullable: true),
                    DestinationBusStationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArrivedTrips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArrivedTrips_BusStations_DestinationBusStationId",
                        column: x => x.DestinationBusStationId,
                        principalTable: "BusStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArrivedTrips_BusStations_OriginBusStationId",
                        column: x => x.OriginBusStationId,
                        principalTable: "BusStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedTrips_DestinationBusStationId",
                table: "ArrivedTrips",
                column: "DestinationBusStationId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivedTrips_OriginBusStationId",
                table: "ArrivedTrips",
                column: "OriginBusStationId");

            migrationBuilder.Sql(
                @"CREATE TRIGGER tr_TripsUpdate
                  ON Trips
                  AFTER UPDATE
                  AS
                  BEGIN
                      INSERT INTO ArrivedTrips
                  	  	     (ArrivalTime, PassengersCount, OriginBusStationId, DestinationBusStationId)
                  	  SELECT t.ArrivalTime,
                  	  	     (SELECT COUNT(*)
                  	  	        FROM Tickets
                  	  		   WHERE TripId = t.Id),
                  	  	     t.OriginBusStationId,
                  	  	     t.DestinationBusStationId
                  	    FROM Trips AS t
                  	  	     JOIN inserted AS i
                  	  	     ON i.Id = t.Id
                  	  	     JOIN deleted AS d
                  	  	     ON d.Id = t.Id
                  	   WHERE d.[Status] <> 'Arrived'
                  	     AND i.[Status] = 'Arrived'
                  END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArrivedTrips");
        }
    }
}
