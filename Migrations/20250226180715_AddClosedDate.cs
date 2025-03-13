using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Togeta.Migrations
{
    /// <inheritdoc />
    public partial class AddClosedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                table: "Events",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedDate",
                table: "Events");
        }
    }
}
