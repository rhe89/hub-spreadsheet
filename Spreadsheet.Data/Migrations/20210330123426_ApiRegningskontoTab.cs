using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class ApiRegningskontoTab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                schema: "dbo",
                table: "SpreadsheetRowMetadata",
                nullable: true);
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetTabMetadata ([SpreadsheetMetadataId], [Name], [FirstColumn], [LastColumn], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'API_REGNINGSKONTO', 'A', 'M', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetMetadata WHERE Name = 'Budget')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "dbo",
                table: "SpreadsheetRowMetadata");
        }
    }
}
