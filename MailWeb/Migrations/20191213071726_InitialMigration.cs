using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MailWeb.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "MailClientSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Availability = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<double>(nullable: false),
                    LastModifiedTime = table.Column<double>(nullable: true),
                    ClientId = table.Column<Guid>(nullable: false),
                    UniqueName = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    Timeout = table.Column<int>(nullable: false),
                    CarbonCopies = table.Column<string>(nullable: true),
                    BlindCarbonCopies = table.Column<string>(nullable: true),
                    MailHost = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailClientSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientSettings_Name",
                table: "ClientSettings",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MailClientSettings_UniqueName",
                table: "MailClientSettings",
                column: "UniqueName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientSettings");

            migrationBuilder.DropTable(
                name: "MailClientSettings");
        }
    }
}
