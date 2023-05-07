using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebbutvecklingLabb2.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "177b4e38-aeee-41e0-83b9-073ea3dd7e6a", "9009a60b-61a4-4296-8ff8-8b3661404846", "Admin", "ADMIN" },
                    { "74c16755-a8a7-4e3c-bd07-669290b33f17", "bb5ca08e-2c88-4037-97a7-e5a4773aa477", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "177b4e38-aeee-41e0-83b9-073ea3dd7e6a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74c16755-a8a7-4e3c-bd07-669290b33f17");
        }
    }
}
