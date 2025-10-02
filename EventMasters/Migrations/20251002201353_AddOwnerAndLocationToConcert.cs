using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMasters.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerAndLocationToConcert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Concert",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Concert",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Concert");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Concert");
        }
    }
}
