using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Togeta.Migrations
{
    /// <inheritdoc />
    public partial class SeedProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "Bio", "Email", "FirstName", "Interests", "LastName", "ProfilePictureUrl" },
                values: new object[,]
                {
                    { 1, "I love coding!", "john.doe@example.com", "John", "C#, .NET, MVC", "Doe", "https://via.placeholder.com/150" },
                    { 2, "Full-stack Developer", "jane.smith@example.com", "Jane", "React, JavaScript, Node.js", "Smith", "https://via.placeholder.com/150" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
