using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class DeleteSpreadsheetMetadataFromSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM dbo.Setting
                WHERE [Key] = 'ApiDataTab' OR [Key] = 'ApiDataTabFirstColumn' OR [Key] = 'ApiDataTabLastColumn' OR [Key] = 'BudgetSpreadsheetId'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}