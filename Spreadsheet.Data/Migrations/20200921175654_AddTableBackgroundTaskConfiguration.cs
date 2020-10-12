using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class AddTableBackgroundTaskConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InitiatedBy",
                schema: "dbo",
                table: "WorkerLog",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BackgroundTaskConfiguration",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LastRun = table.Column<DateTime>(nullable: false),
                    RunInterval = table.Column<int>(nullable: false),
                    RunIntervalType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundTaskConfiguration", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackgroundTaskConfiguration",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "InitiatedBy",
                schema: "dbo",
                table: "WorkerLog");
        }
    }
}
