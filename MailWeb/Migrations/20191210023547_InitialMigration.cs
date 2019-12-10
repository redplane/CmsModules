using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MailWeb.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicMailSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UniqueName = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    Timeout = table.Column<int>(nullable: false),
                    HostName = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    Ssl = table.Column<bool>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicMailSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ActiveMailServiceName = table.Column<string>(nullable: true),
                    ActiveMailServiceType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasicMailSettings_UniqueName",
                table: "BasicMailSettings",
                column: "UniqueName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientSettings_Name",
                table: "ClientSettings",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicMailSettings");

            migrationBuilder.DropTable(
                name: "ClientSettings");
        }
    }
}
