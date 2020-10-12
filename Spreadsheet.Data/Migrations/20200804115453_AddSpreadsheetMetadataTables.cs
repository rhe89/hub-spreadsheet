using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spreadsheet.Data.Migrations
{
    public partial class AddSpreadsheetMetadataTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpreadsheetMetadata",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    SpreadsheetId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidTo = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpreadsheetMetadata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpreadsheetTabMetadata",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    SpreadsheetMetadataId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FirstColumn = table.Column<string>(nullable: true),
                    LastColumn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpreadsheetTabMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpreadsheetTabMetadata_SpreadsheetMetadata_SpreadsheetMetadataId",
                        column: x => x.SpreadsheetMetadataId,
                        principalSchema: "dbo",
                        principalTable: "SpreadsheetMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpreadsheetRowMetadata",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    SpreadsheetTabMetadataId = table.Column<long>(nullable: false),
                    RowKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpreadsheetRowMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpreadsheetRowMetadata_SpreadsheetTabMetadata_SpreadsheetTabMetadataId",
                        column: x => x.SpreadsheetTabMetadataId,
                        principalSchema: "dbo",
                        principalTable: "SpreadsheetTabMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpreadsheetRowMetadata_SpreadsheetTabMetadataId",
                schema: "dbo",
                table: "SpreadsheetRowMetadata",
                column: "SpreadsheetTabMetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_SpreadsheetTabMetadata_SpreadsheetMetadataId",
                schema: "dbo",
                table: "SpreadsheetTabMetadata",
                column: "SpreadsheetMetadataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpreadsheetRowMetadata",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SpreadsheetTabMetadata",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SpreadsheetMetadata",
                schema: "dbo");
        }
    }
}
