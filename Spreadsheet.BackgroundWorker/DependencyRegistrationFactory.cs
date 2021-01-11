using System;
using AutoMapper;
using Hub.HostedServices.Tasks;
using Hub.HostedServices.TimerHost;
using Hub.Storage.Repository.AutoMapper;
using Hub.Web.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spreadsheet.BackgroundTasks;
using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.SpreadsheetTabReaders;
using Spreadsheet.Core.SpreadsheetTabWriters;
using Spreadsheet.Core.Storage;
using Spreadsheet.Data;
using Spreadsheet.Data.AutoMapper;
using Spreadsheet.Data.Storage;
using Spreadsheet.Integration;
using Spreadsheet.Providers;
using Spreadsheet.SpreadsheetTabReaders;
using Spreadsheet.SpreadsheetTabWriters;

namespace Spreadsheet.BackgroundWorker
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactoryBase<SpreadsheetDbContext>
    {
        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IBackgroundTask, UpdateAccountTransfersTask>();
            serviceCollection.AddSingleton<IBackgroundTask, UpdateSbankenAccountsTask>();
            serviceCollection.AddSingleton<IBackgroundTask, UpdateCoinbaseAccountsTask>();
            serviceCollection.AddSingleton<IBackgroundTask, UpdateCoinbaseProAccountsTask>();
            
            serviceCollection.AddSingleton<ITabReader<ResultsAndSavingsTabDto>>(x => 
                new TabReader<ResultsAndSavingsTabDto>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.ResultAndSavingTabName));
            
            serviceCollection.AddSingleton<ITabReader<SbankenAccountsTabDto>>(x => 
                new TabReader<SbankenAccountsTabDto>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.SbankenAccountsTabName));
            
            serviceCollection.AddSingleton<ITabReader<CoinbaseAccountsTabDto>>(x => 
                new TabReader<CoinbaseAccountsTabDto>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.CoinbaseAccountsTabName));
            
            serviceCollection.AddSingleton<ITabReader<CoinbaseProAccountsTabDto>>(x => 
                new TabReader<CoinbaseProAccountsTabDto>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.CoinbaseProAccountsTabName));
            
            serviceCollection.AddSingleton<ITabReader<ApiPaymentsAccountTabDto>>(x => 
                new TabReader<ApiPaymentsAccountTabDto>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.ApiPaymentsAccountTabName));
            
            serviceCollection.AddSingleton<IBankAccountsTabWriter<SbankenAccountsTabDto>, BankAccountsTabWriter<SbankenAccountsTabDto>>();
            serviceCollection.AddSingleton<IBankAccountsTabWriter<CoinbaseAccountsTabDto>, BankAccountsTabWriter<CoinbaseAccountsTabDto>>();
            serviceCollection.AddSingleton<IBankAccountsTabWriter<CoinbaseProAccountsTabDto>, BankAccountsTabWriter<CoinbaseProAccountsTabDto>>();            serviceCollection.AddSingleton<IAzureStorage, AzureStorage>();
            serviceCollection.AddSingleton<ISpreadsheetProvider, SpreadsheetProvider>();
            serviceCollection.AddSingleton<IGoogleSpreadsheetConnector, GoogleSpreadsheetConnector>();
            serviceCollection.AddHubHttpClient<ICoinbaseApiConnector, CoinbaseApiConnector>(client =>
                client.BaseAddress = new Uri(configuration.GetValue<string>("COINBASE_API_HOST")));
            serviceCollection.AddHubHttpClient<ICoinbaseProApiConnector, CoinbaseProApiConnector>(client =>
                client.BaseAddress = new Uri(configuration.GetValue<string>("COINBASE_PRO_API_HOST")));
            serviceCollection.AddHubHttpClient<ISbankenApiConnector, SbankenApiConnector>(client =>
                client.BaseAddress = new Uri(configuration.GetValue<string>("SBANKEN_API_HOST")));
            
            serviceCollection.AddAutoMapper(c =>
            {
                c.AddHostedServiceProfiles();
                c.AddSpreadsheetProfiles();
            });
        }
    }
}