using System;
using Hub.HostedServices.Tasks;
using Hub.HostedServices.Timer;
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

namespace Spreadsheet.BackgroundWorker
{
    public class SpreadsheetWorkerHostBuilder : TimerHostBuilder<SpreadsheetDbContext>
    {
        internal SpreadsheetWorkerHostBuilder(string[] args) : base(args, "SQL_DB_SPREADSHEET")
        {
        }
        
        protected override void RegisterDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IBackgroundTask, UpdateAccountTransfersTask>();
            serviceCollection.AddSingleton<IBackgroundTask, UpdateSbankenBalancesTask>();
            serviceCollection.AddSingleton<IBackgroundTask, UpdateCoinbaseBalancesTask>();
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
    }
}