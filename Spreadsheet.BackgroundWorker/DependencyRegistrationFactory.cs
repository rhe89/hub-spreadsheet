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
using Spreadsheet.Core.Services;
using Spreadsheet.Core.Storage;
using Spreadsheet.Data;
using Spreadsheet.Data.AutoMapper;
using Spreadsheet.Data.Storage;
using Spreadsheet.Integration;
using Spreadsheet.Providers;
using Spreadsheet.Services;

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
            serviceCollection.AddSingleton<IBackgroundTask, UpdateExchangeRatesTask>();
            serviceCollection.AddSingleton<IBackgroundTask, UpdateBillingAccountPaymentsTask>();
            
            serviceCollection.AddSingleton<ITabReaderService<ResultsAndSavingsTab>>(x => 
                new TabReaderService<ResultsAndSavingsTab>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.ResultAndSavingTabName));
            
            serviceCollection.AddSingleton<ITabReaderService<SbankenAccountsTab>>(x => 
                new TabReaderService<SbankenAccountsTab>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.SbankenAccountsTabName));
            
            serviceCollection.AddSingleton<ITabReaderService<CoinbaseAccountsTab>>(x => 
                new TabReaderService<CoinbaseAccountsTab>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.CoinbaseAccountsTabName));
            
            serviceCollection.AddSingleton<ITabReaderService<CoinbaseProAccountsTab>>(x => 
                new TabReaderService<CoinbaseProAccountsTab>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.CoinbaseProAccountsTabName));
            
            serviceCollection.AddSingleton<ITabReaderService<BillingAccountTab>>(x => 
                new TabReaderService<BillingAccountTab>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.BillingAccountTabName));
            
            serviceCollection.AddSingleton<ITabReaderService<ExchangeRatesTab>>(x => 
                new TabReaderService<ExchangeRatesTab>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.ExchangeRatesTabName));
            
            serviceCollection.AddSingleton<ITabDataProvider<SbankenAccountsTab>, BankAccountTabDataProvider<SbankenAccountsTab, ISbankenApiConnector>>();
            serviceCollection.AddSingleton<ITabDataProvider<CoinbaseAccountsTab>, BankAccountTabDataProvider<CoinbaseAccountsTab, ICoinbaseApiConnector>>();
            serviceCollection.AddSingleton<ITabDataProvider<CoinbaseProAccountsTab>, BankAccountTabDataProvider<CoinbaseProAccountsTab, ICoinbaseProApiConnector>>();
            serviceCollection.AddSingleton<ITabDataProvider<ExchangeRatesTab>, ExchangeRatesTabDataProvider>();
            serviceCollection.AddSingleton<ITabDataProvider<BillingAccountTab>, BillingAccountPaymentsProvider>();

            serviceCollection.AddSingleton<ITabWriterService<SbankenAccountsTab>, TabWriterService<SbankenAccountsTab>>();
            serviceCollection.AddSingleton<ITabWriterService<CoinbaseAccountsTab>, TabWriterService<CoinbaseAccountsTab>>();
            serviceCollection.AddSingleton<ITabWriterService<CoinbaseProAccountsTab>, TabWriterService<CoinbaseProAccountsTab>>();
            serviceCollection.AddSingleton<ITabWriterService<ExchangeRatesTab>, TabWriterService<ExchangeRatesTab>>();            
            serviceCollection.AddSingleton<ITabWriterService<BillingAccountTab>, TabWriterService<BillingAccountTab>>();            

            serviceCollection.AddSingleton<IAzureStorage, AzureStorage>();
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