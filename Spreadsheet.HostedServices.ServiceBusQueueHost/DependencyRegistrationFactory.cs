using System;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Settings;
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
            
        serviceCollection.AddTransient<ITabReaderService<BankingAccountsTab>>(x => 
            new TabReaderService<BankingAccountsTab>(x.GetRequiredService<ISpreadsheetMetadataProvider>(),
                x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                SpreadsheetTabMetadataConstants.BankingAccountsTabName));
            
        serviceCollection.AddTransient<ITabReaderService<CryptoAccountsTab>>(x => 
            new TabReaderService<CryptoAccountsTab>(x.GetRequiredService<ISpreadsheetMetadataProvider>(),
                x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                SpreadsheetTabMetadataConstants.CryptoAccountsTabName));
            
        serviceCollection.AddTransient<ITabReaderService<BillingPaymentsTab>>(x => 
            new TabReaderService<BillingPaymentsTab>(x.GetRequiredService<ISpreadsheetMetadataProvider>(),
                x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                SpreadsheetTabMetadataConstants.BillingPaymentsTabName));
            
        serviceCollection.AddTransient<ITabReaderService<ExchangeRatesTab>>(x => 
            new TabReaderService<ExchangeRatesTab>(x.GetRequiredService<ISpreadsheetMetadataProvider>(),
                x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                SpreadsheetTabMetadataConstants.ExchangeRatesTabName));
            
        serviceCollection.AddTransient<ITabDataProvider<BankingAccountsTab>, BankingAccountsTabDataProvider>();
        serviceCollection.AddTransient<ITabDataProvider<CryptoAccountsTab>, CryptoAccountsTabDataProvider>();
        serviceCollection.AddTransient<ITabDataProvider<ExchangeRatesTab>, ExchangeRatesTabDataProvider>();
        serviceCollection.AddTransient<ITabDataProvider<BillingPaymentsTab>, BillingPaymentsTabDataProvider>();

        serviceCollection.AddTransient<ITabWriterService<BankingAccountsTab>, TabWriterService<BankingAccountsTab>>();
        serviceCollection.AddTransient<ITabWriterService<CryptoAccountsTab>, TabWriterService<CryptoAccountsTab>>();
        serviceCollection.AddTransient<ITabWriterService<ExchangeRatesTab>, TabWriterService<ExchangeRatesTab>>();            
        serviceCollection.AddTransient<ITabWriterService<BillingPaymentsTab>, TabWriterService<BillingPaymentsTab>>();

        serviceCollection.AddTransient<ISpreadsheetMetadataProvider, SpreadsheetMetadataProvider>();
        serviceCollection.AddTransient<IGoogleSpreadsheetConnector, GoogleSpreadsheetConnector>();
        serviceCollection.AddHubHttpClient<ICryptoApiConnector, CryptoApiConnector>(client =>
            client.BaseAddress = new Uri(configuration.GetValue<string>(ApiEndpoints.Crypto)));
        serviceCollection.AddHubHttpClient<IBankingApiConnector, BankingApiConnector>(client =>
            client.BaseAddress = new Uri(configuration.GetValue<string>(ApiEndpoints.Banking)));
            
        serviceCollection.AddAutoMapper(c =>
        {
            c.AddSpreadsheetProfiles();
        });
    }

    protected override void AddQueueListenerServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient<UpdateBillingPaymentsCommand>();
        serviceCollection.AddTransient<UpdateCryptoAccountsCommand>();
        serviceCollection.AddTransient<UpdateExchangeRatesCommand>();
        serviceCollection.AddTransient<UpdateBankingAccountsCommand>();
            
        serviceCollection.AddHostedService<BillingPaymentsUpdatedService>();
        serviceCollection.AddHostedService<CryptoAccountsUpdatedQueueListener>();
        serviceCollection.AddHostedService<ExchangeRatesUpdatedService>();
        serviceCollection.AddHostedService<BankingAccountsUpdatedQueueListener>();
    }
}