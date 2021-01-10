using System;
using System.IO;
using System.Threading.Tasks;
using Hub.Web.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.SpreadsheetTabReaders;
using Spreadsheet.Core.SpreadsheetTabWriters;
using Spreadsheet.Core.Storage;
using Spreadsheet.Data;
using Spreadsheet.Data.Storage;
using Spreadsheet.Integration;
using Spreadsheet.Providers;
using Spreadsheet.SpreadsheetTabReaders;
using Spreadsheet.SpreadsheetTabWriters;

namespace Spreadsheet.Console
{
    class Program
    {
        static Task Main(string[] args)
        {
            var configPath = $"{Directory.GetCurrentDirectory()}/../..";
            
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(configPath)
                .AddJsonFile("appsettings.json", true)
                .Build();

            IHost host = CreateHostBuilder(args)
                .ConfigureServices((_, services) => RegisterDomainDependencies(services, config))
                .Build(); 

            return host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) 
            => Host.CreateDefaultBuilder(args);

        private static void RegisterDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            
            serviceCollection.AddSingleton<ITabReader<ResultsAndSavingsTabDto>>(x => 
                new TabReader<ResultsAndSavingsTabDto>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.ResultAndSavingTabName));
            
            serviceCollection.AddSingleton<ITabReader<SbankenAccountsTabDto>>(x => 
                new TabReader<SbankenAccountsTabDto>(x.GetRequiredService<ISpreadsheetProvider>(),
                    x.GetRequiredService<IGoogleSpreadsheetConnector>(), 
                    SpreadsheetTabMetadataConstants.ApiDataTabName));
            
            serviceCollection.AddSingleton<IBankAccountsBalanceTabWriter<SbankenAccountsTabDto>, BankAccountsBalanceTabWriter<SbankenAccountsTabDto>>();
            serviceCollection.AddSingleton<IBankAccountsBalanceTabWriter<CoinbaseAccountsTabDto>, BankAccountsBalanceTabWriter<CoinbaseAccountsTabDto>>();
            serviceCollection.AddSingleton<IBankAccountsBalanceTabWriter<CoinbaseProAccountsTabDto>, BankAccountsBalanceTabWriter<CoinbaseProAccountsTabDto>>();
            serviceCollection.AddSingleton<IAzureStorage, AzureStorage>();
            serviceCollection.AddSingleton<ISpreadsheetProvider, SpreadsheetProvider>();
            serviceCollection.AddSingleton<IGoogleSpreadsheetConnector, GoogleSpreadsheetConnector>();
            serviceCollection.AddHubHttpClient<ICoinbaseApiConnector, CoinbaseApiConnector>(client =>
                client.BaseAddress = new Uri(configuration.GetValue<string>("COINBASE_API_HOST")));
            serviceCollection.AddHubHttpClient<ISbankenApiConnector, SbankenApiConnector>(client =>
                client.BaseAddress = new Uri(configuration.GetValue<string>("SBANKEN_API_HOST")));
            serviceCollection.AddDbContext<SpreadsheetDbContext>(options => 
                options.UseSqlServer(configuration.GetValue<string>("sdfdsf"), 
                    x => x.MigrationsAssembly("Spreadsheet.Data")));
        }
    }
    
    
}