using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("09c17917-b264-413d-8c72-d268bfcf9cd1"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("09c17917-b264-413d-8c72-d268bfcf9cd1"), null, "Admin", "ADMIN" });
        }
    }
}
