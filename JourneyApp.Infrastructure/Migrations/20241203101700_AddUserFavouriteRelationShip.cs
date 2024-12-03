using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFavouriteRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TripUser",
                columns: table => new
                {
                    FavoriteTripsId = table.Column<Guid>(type: "uuid", nullable: false),
                    FavoritedByUsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripUser", x => new { x.FavoriteTripsId, x.FavoritedByUsersId });
                    table.ForeignKey(
                        name: "FK_TripUser_AspNetUsers_FavoritedByUsersId",
                        column: x => x.FavoritedByUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripUser_Trips_FavoriteTripsId",
                        column: x => x.FavoriteTripsId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripUser_FavoritedByUsersId",
                table: "TripUser",
                column: "FavoritedByUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripUser");
        }
    }
}
