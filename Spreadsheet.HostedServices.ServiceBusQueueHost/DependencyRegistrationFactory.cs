using System;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Web.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spreadsheet.Shared.Constants;
using Spreadsheet.Data;
using Spreadsheet.Data.AutoMapper;
using Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;
using Spreadsheet.HostedServices.ServiceBusQueueHost.QueueListeners;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost;

public class DependencyRegistrationFactory : DependencyRegistrationFactory<SpreadsheetDbContext>
{
    protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient<ISpreadsheetCosmosDb, SpreadsheetCosmosDb>();

        serviceCollection.AddTransient<ITabReaderService<ResultsAndSavingsTab>>(x => 
            new TabReaderService<ResultsAndSavingsTab>(x.GetRequiredService<ISpreadsheetMetadataProvider>(),
                x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                SpreadsheetTabMetadataConstants.ResultAndSavingTabName));
            
        serviceCollection.AddTransient<ITabReaderService<SbankenAccountsTab>>(x => 
            new TabReaderService<SbankenAccountsTab>(x.GetRequiredService<ISpreadsheetMetadataProvider>(),
                x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                SpreadsheetTabMetadataConstants.SbankenAccountsTabName));
            
        serviceCollection.AddTransient<ITabReaderService<CoinbaseProAccountsTab>>(x => 
            new TabReaderService<CoinbaseProAccountsTab>(x.GetRequiredService<ISpreadsheetMetadataProvider>(),
                x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                SpreadsheetTabMetadataConstants.CoinbaseProAccountsTabName));
            
        serviceCollection.AddTransient<ITabReaderService<BillingAccountTab>>(x => 
            new TabReaderService<BillingAccountTab>(x.GetRequiredService<ISpreadsheetMetadataProvider>(),
                x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                SpreadsheetTabMetadataConstants.BillingAccountTabName));
            
        serviceCollection.AddTransient<ITabReaderService<ExchangeRatesTab>>(x => 
            new TabReaderService<ExchangeRatesTab>(x.GetRequiredService<ISpreadsheetMetadataProvider>(),
                x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                SpreadsheetTabMetadataConstants.ExchangeRatesTabName));
            
        serviceCollection.AddTransient<ITabDataProvider<SbankenAccountsTab>, BankAccountTabDataProvider<SbankenAccountsTab, ISbankenApiConnector>>();
        serviceCollection.AddTransient<ITabDataProvider<CoinbaseProAccountsTab>, BankAccountTabDataProvider<CoinbaseProAccountsTab, ICoinbaseProApiConnector>>();
        serviceCollection.AddTransient<ITabDataProvider<ExchangeRatesTab>, ExchangeRatesTabDataProvider>();
        serviceCollection.AddTransient<ITabDataProvider<BillingAccountTab>, BillingAccountTransactionsProvider>();

        serviceCollection.AddTransient<ITabWriterService<SbankenAccountsTab>, TabWriterService<SbankenAccountsTab>>();
        serviceCollection.AddTransient<ITabWriterService<CoinbaseProAccountsTab>, TabWriterService<CoinbaseProAccountsTab>>();
        serviceCollection.AddTransient<ITabWriterService<ExchangeRatesTab>, TabWriterService<ExchangeRatesTab>>();            
        serviceCollection.AddTransient<ITabWriterService<BillingAccountTab>, TabWriterService<BillingAccountTab>>();

        serviceCollection.AddTransient<ISpreadsheetMetadataProvider, SpreadsheetMetadataProvider>();
        serviceCollection.AddTransient<IGoogleSpreadsheetConnector, GoogleSpreadsheetConnector>();
        serviceCollection.AddHubHttpClient<ICoinbaseApiConnector, CoinbaseApiConnector>(client =>
            client.BaseAddress = new Uri(configuration.GetValue<string>("COINBASE_API_HOST")));
        serviceCollection.AddHubHttpClient<ICoinbaseProApiConnector, CoinbaseProApiConnector>(client =>
            client.BaseAddress = new Uri(configuration.GetValue<string>("COINBASE_PRO_API_HOST")));
        serviceCollection.AddHubHttpClient<ISbankenApiConnector, SbankenApiConnector>(client =>
            client.BaseAddress = new Uri(configuration.GetValue<string>("SBANKEN_API_HOST")));
            
        serviceCollection.AddAutoMapper(c =>
        {
            c.AddSpreadsheetProfiles();
        });
    }

    protected override void AddQueueListenerServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient<UpdateBillingAccountTransactionsCommand>();
        serviceCollection.AddTransient<UpdateCoinbaseProAccountsCommand>();
        serviceCollection.AddTransient<UpdateExchangeRatesCommand>();
        serviceCollection.AddTransient<UpdateSbankenAccountsCommand>();
            
        serviceCollection.AddHostedService<BillingAccountsTransactionsUpdatedService>();
        serviceCollection.AddHostedService<CoinbaseProAccountsUpdatedQueueListener>();
        serviceCollection.AddHostedService<ExchangeRatesUpdatedService>();
        serviceCollection.AddHostedService<SbankenAccountsUpdatedQueueListener>();
    }
}