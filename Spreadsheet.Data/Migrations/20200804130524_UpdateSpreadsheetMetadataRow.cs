using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class UpdateSpreadsheetMetadataRow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"UPDATE dbo.SpreadsheetRowMetadata SET RowKey = 'VISA KREDITTKORT' WHERE RowKey = 'Kredittkort'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
