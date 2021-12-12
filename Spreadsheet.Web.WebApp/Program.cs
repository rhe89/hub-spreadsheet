using FluentValidation;
using FluentValidation.AspNetCore;
using Hub.Shared.Web.BlazorServer;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Extensions.DependencyInjection;
using Spreadsheet.Data;
using Spreadsheet.Data.AutoMapper;
using Spreadsheet.Data.Dto;
using Spreadsheet.Providers;
using Spreadsheet.Services;
using Spreadsheet.Web.WebApp;
using Spreadsheet.Web.WebApp.Validation;

var builder = BlazorServerBuilder.CreateWebApplicationBuilder(args);

builder.Services.AddTransient<ISpreadsheetCosmosDb, SpreadsheetCosmosDb>();
builder.Services.AddTransient<ISpreadsheetMetadataProvider, SpreadsheetMetadataProvider>();
builder.Services.AddTransient<ISpreadsheetMetadataService, SpreadsheetMetadataService>();
builder.Services.AddTransient<IValidator<SpreadsheetMetadataDto>, SpreadsheetMetadataValidator>();
builder.Services.AddSingleton<State>();

builder.Services.AddAutoMapper(c => { c.AddSpreadsheetProfiles(); });

builder.Services.AddFluentValidation();
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var app = builder.BuildApp();

app.Run();