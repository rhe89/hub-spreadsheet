using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class ExchangeRatesTab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetTabMetadata ([SpreadsheetMetadataId], [Name], [FirstColumn], [LastColumn], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'EXCHANGE_RATES', 'A', 'M', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetMetadata WHERE Name = 'Budget')");

            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'BTC', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'EXCHANGE_RATES')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'ETH', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'EXCHANGE_RATES')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
