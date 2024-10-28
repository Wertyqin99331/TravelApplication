using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropertyNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description_Value",
                table: "Trips",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "City_Value",
                table: "Trips",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "ReviewText_Value",
                table: "TripReviews",
                newName: "ReviewText");

            migrationBuilder.RenameColumn(
                name: "Rating_Value",
                table: "TripReviews",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "Title_Value",
                table: "TripPlaces",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Description_Value",
                table: "TripPlaces",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "City_Value",
                table: "TripDays",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "Surname_Value",
                table: "AspNetUsers",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "Name_Value",
                table: "AspNetUsers",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Trips",
                newName: "Description_Value");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Trips",
                newName: "City_Value");

            migrationBuilder.RenameColumn(
                name: "ReviewText",
                table: "TripReviews",
                newName: "ReviewText_Value");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "TripReviews",
                newName: "Rating_Value");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TripPlaces",
                newName: "Title_Value");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TripPlaces",
                newName: "Description_Value");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "TripDays",
                newName: "City_Value");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "AspNetUsers",
                newName: "Surname_Value");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "Name_Value");
        }
    }
}
