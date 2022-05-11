using Hub.Shared.Web.Api;
using Spreadsheet.Data;

var builder = WebApiBuilder.CreateWebApplicationBuilder<SpreadsheetDbContext>(args, "SQL_DB_SPREADSHEET");

builder
    .BuildApp()
    .Run();
