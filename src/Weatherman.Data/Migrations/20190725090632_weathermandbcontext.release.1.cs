using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Weatherman.Data.Migrations
{
    public partial class weathermandbcontextrelease1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "server_profile",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    prefix = table.Column<string>(nullable: true),
                    last_changed_by = table.Column<string>(nullable: true),
                    last_changed_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_server_profile", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_profile",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    home_location = table.Column<string>(nullable: true),
                    last_location = table.Column<string>(nullable: true),
                    home_location_changed_date = table.Column<DateTime>(nullable: true),
                    last_location_changed_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user_profile", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "server_profile");

            migrationBuilder.DropTable(
                name: "user_profile");
        }
    }
}
