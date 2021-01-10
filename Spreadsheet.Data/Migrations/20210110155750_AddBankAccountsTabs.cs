using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class AddBankAccountsTabs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetTabMetadata ([SpreadsheetMetadataId], [Name], [FirstColumn], [LastColumn], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'SBANKEN_ACCOUNTS', 'A', 'M', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetMetadata WHERE Name = 'Budget')");

            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Brukskonto', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'SBANKEN_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Regningsbetaling', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'SBANKEN_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Buffersparing', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'SBANKEN_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Snuskonto', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'SBANKEN_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Kredittkort', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'SBANKEN_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Lønnskonto', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'SBANKEN_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Feriesparing', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'SBANKEN_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Kjøpskonto', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'SBANKEN_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Felleskonto', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'SBANKEN_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Sist oppdatert', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'SBANKEN_ACCOUNTS')");
            
            // COINBASE_ACCOUNTS
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetTabMetadata ([SpreadsheetMetadataId], [Name], [FirstColumn], [LastColumn], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'COINBASE_ACCOUNTS', 'A', 'M', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetMetadata WHERE Name = 'Budget')");

            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'EUR', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'BTC', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'ETH', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'LTC', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'BCH', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'ZRX', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'BAT', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'ZEC', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'BSV', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'ETC', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'USD', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'XRP', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'USDC', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Sist oppdatert', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_ACCOUNTS')");
            
            // COINBASE_PRO_ACCOUNTS
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetTabMetadata ([SpreadsheetMetadataId], [Name], [FirstColumn], [LastColumn], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'COINBASE_PRO_ACCOUNTS', 'A', 'M', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetMetadata WHERE Name = 'Budget')");

            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'BTC', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_PRO_ACCOUNTS')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'EUR', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'COINBASE_PRO_ACCOUNTS')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
