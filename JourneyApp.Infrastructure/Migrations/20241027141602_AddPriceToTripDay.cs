using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToTripDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Trips",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "TripPlaces",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "TripPlaces");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Trips",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
