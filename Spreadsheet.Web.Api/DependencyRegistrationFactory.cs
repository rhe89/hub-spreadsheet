using System;
using Hub.Web.DependencyRegistration;
using Hub.Web.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spreadsheet.BackgroundTasks;
using Spreadsheet.Data;
using Spreadsheet.Data.Storage;
using Spreadsheet.Integration;
using Spreadsheet.Providers;
using Spreadsheet.SpreadsheetTabReaders;
using Spreadsheet.SpreadsheetTabWriters;

namespace Spreadsheet.Web.Api
{
    public class DependencyRegistrationFactory : ApiWithQueueHostedServiceDependencyRegistrationFactory<SpreadsheetDbContext>
    {

        public DependencyRegistrationFactory() : base("SQL_DB_SPREADSHEET", "Spreadsheet.Data")
        {
        }

        protected override void RegisterDomainDependenciesForQueueHostedService(IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddSingleton<UpdateAccountTransfersTask>();
            serviceCollection.AddSingleton<UpdateSbankenBalancesTask>();
            serviceCollection.AddSingleton<UpdateCoinbaseBalancesTask>();
            serviceCollection.AddSingleton<IResultOgSavingsTabReader, ResultOgSavingsTabReader>();
            serviceCollection.AddSingleton<IApiDataTabReader, ApiDataTabReader>();
            serviceCollection.AddSingleton<IApiDataTabWriter, ApiDataTabWriter>();
            serviceCollection.AddSingleton<IAzureStorage, AzureStorage>();
            serviceCollection.AddSingleton<ISpreadsheetProvider, SpreadsheetProvider>();
            serviceCollection.AddSingleton<IGoogleSpreadsheetConnector, GoogleSpreadsheetConnector>();
            serviceCollection.AddHubHttpClient<ICoinbaseApiConnector, CoinbaseApiConnector>(client =>
                client.BaseAddress = new Uri(configuration.GetValue<string>("COINBASE_API_HOST")));
            serviceCollection.AddHubHttpClient<ISbankenApiConnector, SbankenApiConnector>(client =>
                client.BaseAddress = new Uri(configuration.GetValue<string>("SBANKEN_API_HOST")));
        }

        protected override void RegisterSharedDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            // Not needed since all dependencies are registered elsewhere
        }
        
        protected override void RegisterDomainDependenciesForApi(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            // Not needed since all dependencies are registered elsewhere
        }
    }
}