using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MailWeb.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "ClientSettings",
                table => new
                {
                    Id = table.Column<Guid>(),
                    Name = table.Column<string>(),
                    ActiveMailClient = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_ClientSettings", x => x.Id); });

            migrationBuilder.CreateTable(
                "MailClientSettings",
                table => new
                {
                    Id = table.Column<Guid>(),
                    Availability = table.Column<int>(),
                    CreatedTime = table.Column<double>(),
                    LastModifiedTime = table.Column<double>(nullable: true),
                    ClientId = table.Column<Guid>(),
                    UniqueName = table.Column<string>(),
                    DisplayName = table.Column<string>(nullable: true),
                    Timeout = table.Column<int>(),
                    CarbonCopies = table.Column<string>(nullable: true),
                    BlindCarbonCopies = table.Column<string>(nullable: true),
                    MailHost = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_MailClientSettings", x => x.Id); });

            migrationBuilder.CreateIndex(
                "IX_ClientSettings_Name",
                "ClientSettings",
                "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_MailClientSettings_UniqueName",
                "MailClientSettings",
                "UniqueName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "ClientSettings");

            migrationBuilder.DropTable(
                "MailClientSettings");
        }
    }
}