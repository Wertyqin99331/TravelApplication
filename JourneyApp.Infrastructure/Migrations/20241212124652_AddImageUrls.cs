using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Trips",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "TripPlaces",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "TripPlaces");
        }
    }
}
