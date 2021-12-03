using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class SpreadsheetJsonDataStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Data",
                schema: "dbo",
                table: "SpreadsheetMetadata",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE dbo.SpreadsheetMetadata 
                SET Data =
                    (SELECT *
                     FROM dbo.SpreadsheetMetadata sm
                INNER JOIN dbo.SpreadsheetTabMetadata Tabs on Tabs.SpreadsheetMetadataId = sm.Id
                INNER JOIN dbo.SpreadsheetRowMetadata Rows on Rows.SpreadsheetTabMetadataId = Tabs.Id 
                WHERE sm.Id = sm1.Id
                FOR JSON AUTO)
                FROM dbo.SpreadsheetMetadata sm1");

            migrationBuilder.DropTable("SpreadsheetRowMetadata", "dbo");
            migrationBuilder.DropTable("SpreadsheetTabMetadata", "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                schema: "dbo",
                table: "SpreadsheetMetadata");
        }
    }
}
