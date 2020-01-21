using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MailWeb.Migrations
{
    public partial class AddSiteIdIntoCorsPolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InUseCorsPolicies",
                table: "ClientSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "SiteId",
                table: "CorsPolicies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "CorsPolicies");

            migrationBuilder.AddColumn<string>(
                name: "InUseCorsPolicies",
                table: "ClientSettings",
                nullable: true);
        }
    }
}
