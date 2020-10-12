using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class RemoveColumnRunInterval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RunInterval",
                schema: "dbo",
                table: "BackgroundTaskConfiguration");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RunInterval",
                schema: "dbo",
                table: "BackgroundTaskConfiguration",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
