using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsModulesManagement.Migrations
{
    public partial class RemoveActiveMailClientFromSiteSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveMailClient",
                table: "ClientSettings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveMailClient",
                table: "ClientSettings",
                nullable: true);
        }
    }
}
