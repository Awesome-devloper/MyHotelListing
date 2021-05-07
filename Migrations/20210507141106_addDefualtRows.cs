using Microsoft.EntityFrameworkCore.Migrations;

namespace MyHotelListing.Migrations
{
    public partial class addDefualtRows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1fd06ca9-335c-47a7-a573-e5902e10fccf", "091d07ca-a49a-4f23-8eab-69b6385d1740", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b9cc2e45-67b8-46d4-a304-1e3a6103d287", "f449bde4-490a-44d0-8e32-4788be64d39d", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fd06ca9-335c-47a7-a573-e5902e10fccf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9cc2e45-67b8-46d4-a304-1e3a6103d287");
        }
    }
}
