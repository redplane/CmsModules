using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsModulesManagement.Migrations
{
    public partial class RemoveSiteIdColumnFromSiteCorsPolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "CorsPolicies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SiteId",
                table: "CorsPolicies",
                nullable: true);
        }
    }
}
