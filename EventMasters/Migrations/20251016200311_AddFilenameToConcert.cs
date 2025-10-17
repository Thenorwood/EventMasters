using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMasters.Migrations
{
    /// <inheritdoc />
    public partial class AddFilenameToConcert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "Concert",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Filename",
                table: "Concert");
        }
    }
}
