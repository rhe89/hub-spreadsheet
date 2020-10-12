using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class InsertSpreadsheetMetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetMetadata ([SpreadsheetId], [Name], [ValidFrom], [ValidTo], [CreatedDate], [UpdatedDate]) 
                VALUES ('1134z6195n8U_jRx1lCJOfd4Ync0dCBChauqlTJbyGE8', 'Budget', '{new DateTime(DateTime.Now.Year, 1, 1)}', '{new DateTime(DateTime.Now.Year, 12, 31)}', '{DateTime.Now}', '{DateTime.Now}'),
                       ('1SFdSU5UbP4Qxf5bzn0e1Lj3noz2iYAcNVow-rlCLOlw', 'Budget', '{new DateTime(DateTime.Now.Year+1, 1, 1)}', '{new DateTime(DateTime.Now.Year+1, 12, 31)}', '{DateTime.Now}', '{DateTime.Now}')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetTabMetadata ([SpreadsheetMetadataId], [Name], [FirstColumn], [LastColumn], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'API_Data', 'A', 'ZZ', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetMetadata WHERE Name = 'Budget')");

            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Brukskonto', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Regningsbetaling', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Buffersparing', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Snuskonto', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Kredittkort', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Lønnskonto', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Feriesparing', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Kjøpskonto', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Kryptovaluta', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Felleskonto', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
            
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.SpreadsheetRowMetadata ([SpreadsheetTabMetadataId], [RowKey], [CreatedDate], [UpdatedDate])
                    (SELECT Id, 'Sist oppdatert', '{DateTime.Now}', '{DateTime.Now}' FROM dbo.SpreadsheetTabMetadata WHERE Name = 'API_Data')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
