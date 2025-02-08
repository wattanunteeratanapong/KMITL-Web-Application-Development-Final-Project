using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Togeta.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverPhotoUrl",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CoverPhotoUrl",
                value: "https://source.unsplash.com/800x300/?technology");

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CoverPhotoUrl",
                value: "https://source.unsplash.com/800x300/?nature");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPhotoUrl",
                table: "Profiles");
        }
    }
}
