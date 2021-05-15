using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class DbCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandLog",
                schema: "dbo");

            migrationBuilder.Sql("DELETE FROM dbo.CommandConfiguration");
            
            migrationBuilder.Sql(@"
             DELETE FROM dbo.Setting
             WHERE [Key] = 'BudgetWorkerRunInterval' OR
                   [Key] = 'LogMaintenanceWorkerRunInterval' OR
                   [Key] = 'SbankenApiHost' OR
                   [Key] = 'CoinbaseApiHost' OR
                   [Key] = 'AgeInDaysOfWorkerLogsToDelete' OR
                   [Key] = 'StorageAccount'");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommandLog",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommandName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitiatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandLog", x => x.Id);
                });
        }
    }
}
