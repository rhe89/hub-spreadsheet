using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.SpreadsheetTabWriters;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateExchangeRatesTask : BackgroundTask
    {
        private readonly IExchangeRatesTabWriter _exchangeRatesTabWriter;
        private readonly ICoinbaseApiConnector _coinbaseApiConnector;
        private readonly ILogger<UpdateExchangeRatesTask> _logger;

        public UpdateExchangeRatesTask(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider, 
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            IExchangeRatesTabWriter exchangeRatesTabWriter,
            ICoinbaseApiConnector coinbaseApiConnector,
            ILogger<UpdateExchangeRatesTask> logger) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _exchangeRatesTabWriter = exchangeRatesTabWriter;
            _coinbaseApiConnector = coinbaseApiConnector;
            _logger = logger;
        }
        
        public override async Task Execute(CancellationToken cancellationToken)
        {
            var bankAccounts = await GetExchangeRates();

            if (bankAccounts == null)
            {
                return;
            }

            var bankAccountsTaskLastRun = await GetUpdateExchangeRatesTaskInCoinbaseContainerLastRun();

            await _exchangeRatesTabWriter.UpdateTab(bankAccounts, bankAccountsTaskLastRun);
        }


        private async Task<IList<ExchangeRateDto>> GetExchangeRates()
        {
            _logger.LogInformation($"Getting accounts from {_coinbaseApiConnector.FriendlyApiName}");

            var response = await _coinbaseApiConnector.GetExchangeRates();

            if (!response.Success)
            {
                throw new Exception(
                    $"GetBankAccounts: {_coinbaseApiConnector.FriendlyApiName}: {response.ErrorMessage}");
            }

            var bankAccounts = response.Data;
            
            _logger.LogInformation($"Got {bankAccounts.Count} accounts from {_coinbaseApiConnector.FriendlyApiName}");

            return bankAccounts;
        }
        
        private async Task<DateTime> GetUpdateExchangeRatesTaskInCoinbaseContainerLastRun()
        {
            var response = await _coinbaseApiConnector.GetBackgroundTaskConfigurations();

            if (!response.Success)
            {
                throw new Exception(
                    $"GetBankAccountsTaskLastRun: {_coinbaseApiConnector.FriendlyApiName}: {response.ErrorMessage}");
            }
            
            var bankAccountsTask = response.Data
                ?.FirstOrDefault(x => x.Name == "UpdateExchangeRatesTask");

            return bankAccountsTask?.LastRun ?? DateTime.MinValue;
        }
    }
}