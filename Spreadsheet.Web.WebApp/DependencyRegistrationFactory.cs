using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spreadsheet.Data;
using Spreadsheet.Data.AutoMapper;
using Spreadsheet.Data.Dto;
using Spreadsheet.Providers;
using Spreadsheet.Services;
using Spreadsheet.Web.WebApp.Validation;

namespace Spreadsheet.Web.WebApp
{
    public class DependencyRegistrationFactory : Hub.Shared.Web.BlazorServer.DependencyRegistrationFactory
    {
        protected override void AddBlazorExtras(IServiceCollection serviceCollection, IConfiguration configuration)
        {
        }

        protected override void AddHttpClients(IServiceCollection serviceCollection, IConfiguration configuration)
        {
        }

        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<ISpreadsheetCosmosDb, SpreadsheetCosmosDb>();
            serviceCollection.AddTransient<ISpreadsheetMetadataProvider, SpreadsheetMetadataProvider>();
            serviceCollection.AddTransient<ISpreadsheetMetadataService, SpreadsheetMetadataService>();
            serviceCollection.AddSingleton<State>();

            serviceCollection.AddAutoMapper(c => { c.AddSpreadsheetProfiles(); });

            serviceCollection.AddFluentValidation();
            serviceCollection.AddTransient<IValidator<SpreadsheetMetadataDto>, SpreadsheetMetadataValidator>();
        }
    }
}