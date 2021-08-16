using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcUp1.Migrations
{
    public partial class UpdateApplicationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Application",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Application",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Application",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Application",
                newName: "id");
        }
    }
}
