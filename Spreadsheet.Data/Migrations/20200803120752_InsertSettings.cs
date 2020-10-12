using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class InsertSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @$"INSERT INTO dbo.Setting ([Key], [Value], [CreatedDate], [UpdatedDate]) 
                VALUES ('StorageAccount', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('StorageAccountFileShare', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('StorageAccountFileShareCertificateFolder', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('GoogleCertificate', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('GooglePrivateKey', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('GoogleServiceAccountEmail', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('BudgetSpreadsheetId', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('ApiDataTab', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('ApiDataTabFirstColumn', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('ApiDataTabLastColumn', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('SbankenApiHost', '', '{DateTime.Now}', '{DateTime.Now}'),
                       ('CoinbaseApiHost', '', '{DateTime.Now}', '{DateTime.Now}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
