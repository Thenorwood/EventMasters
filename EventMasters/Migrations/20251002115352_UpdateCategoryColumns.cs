using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMasters.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Category",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Category",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Category",
                newName: "Title");
        }
    }
}
