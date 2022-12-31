using Hub.Shared.Web.Api;
using Microsoft.Extensions.DependencyInjection;
using Spreadsheet.Data;
using Spreadsheet.Integration;
using Spreadsheet.Providers;

var builder = WebApiBuilder.CreateWebApplicationBuilder(args);

builder.Services.AddTransient<ICosmosDbContext, CosmosDbContextContext>();

builder.Services.AddTransient<IGoogleSpreadsheetConnector, GoogleSpreadsheetConnector>();
builder.Services.AddTransient<ISpreadsheetMetadataProvider, SpreadsheetMetadataProvider>();
builder.Services.AddTransient<IIncomeTabProvider, IncomeTabProvider>();
builder.Services.AddTransient<IDebtTabProvider, DebtTabProvider>();

builder
    .BuildApp()
    .Run();
