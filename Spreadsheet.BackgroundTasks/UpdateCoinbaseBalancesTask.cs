using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Factories;
using Hub.Storage.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Dto.Account;
using Spreadsheet.Integration;
using Spreadsheet.SpreadsheetTabWriters;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateCoinbaseBalancesTask : BackgroundTask
    {
        private readonly ILogger<UpdateCoinbaseBalancesTask> _logger;
        private readonly ICoinbaseApiConnector _coinbaseApiConnector;
        private readonly IApiDataTabWriter _apiDataTabWriter;

        public UpdateCoinbaseBalancesTask(ILogger<UpdateCoinbaseBalancesTask> logger, 
            ICoinbaseApiConnector coinbaseApiConnector,
            IApiDataTabWriter apiDataTabWriter, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _logger = logger;
            _coinbaseApiConnector = coinbaseApiConnector;
            _apiDataTabWriter = apiDataTabWriter;
        }
        public override async Task Execute(CancellationToken cancellationToken)
        {
            var account = await GetAccount();

            if (account == null)
                return;
            
            await _apiDataTabWriter.UpdateTab(account);
        }

        private async Task<AccountDto> GetAccount()
        {
            var coinbaseAccounts = await GetCoinbaseAccounts();

            if (coinbaseAccounts == null)
                return null;

            var summaryAccount = coinbaseAccounts.FirstOrDefault(x => x.Name == "Total");

            return new AccountDto
            {
                Name = "Kryptovaluta",
                Balance = summaryAccount?.Balance ?? 0
            };
        }

        private async Task<IList<AccountDto>> GetCoinbaseAccounts()
        {
            _logger.LogInformation("Getting accounts from Coinbase API");

            var response = await _coinbaseApiConnector.GetAccounts();

            if (!response.Success)
                return null;

            var coinbaseAccounts = response.Data;
            
            _logger.LogInformation($"Got {coinbaseAccounts.Count()} accounts from Coinbase API");

            return coinbaseAccounts;
        }
    }
}