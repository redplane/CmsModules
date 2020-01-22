using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsModulesManagement.Migrations
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
                    ActiveMailClient = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CorsPolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Availability = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<double>(nullable: false),
                    LastModifiedTime = table.Column<double>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    AllowedHeaders = table.Column<string>(nullable: true),
                    AllowedOrigins = table.Column<string>(nullable: true),
                    AllowedMethods = table.Column<string>(nullable: true),
                    AllowedExposedHeaders = table.Column<string>(nullable: true),
                    AllowCredential = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorsPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteMailClientSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Availability = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<double>(nullable: false),
                    LastModifiedTime = table.Column<double>(nullable: true),
                    UniqueName = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    Timeout = table.Column<int>(nullable: false),
                    CarbonCopies = table.Column<string>(nullable: true),
                    BlindCarbonCopies = table.Column<string>(nullable: true),
                    MailHost = table.Column<string>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteMailClientSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientSettings_Name",
                table: "ClientSettings",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CorsPolicies_Name",
                table: "CorsPolicies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteMailClientSettings_UniqueName_TenantId",
                table: "SiteMailClientSettings",
                columns: new[] { "UniqueName", "TenantId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientSettings");

            migrationBuilder.DropTable(
                name: "CorsPolicies");

            migrationBuilder.DropTable(
                name: "SiteMailClientSettings");
        }
    }
}
