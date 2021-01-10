using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.BackgroundTasks;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.SpreadsheetTabWriters;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateCoinbaseAccountsTask : BackgroundTask
    {
        private readonly ILogger<UpdateCoinbaseAccountsTask> _logger;
        private readonly ICoinbaseApiConnector _coinbaseApiConnector;
        private readonly IBankAccountsBalanceTabWriter<CoinbaseAccountsTabDto> _coinbaseAccountsBalanceTabWriter;

        public UpdateCoinbaseAccountsTask(ILogger<UpdateCoinbaseAccountsTask> logger, 
            ICoinbaseApiConnector coinbaseApiConnector,
            IBankAccountsBalanceTabWriter<CoinbaseAccountsTabDto> coinbaseAccountsBalanceTabWriter, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _logger = logger;
            _coinbaseApiConnector = coinbaseApiConnector;
            _coinbaseAccountsBalanceTabWriter = coinbaseAccountsBalanceTabWriter;
        }
        public override async Task Execute(CancellationToken cancellationToken)
        {
            var accounts = await GetCoinbaseAccounts();

            if (accounts == null)
            {
                return;
            }
            
            await _coinbaseAccountsBalanceTabWriter.UpdateTab(accounts);
        }

        private async Task<IList<AccountDto>> GetCoinbaseAccounts()
        {
            _logger.LogInformation("Getting accounts from Coinbase API");

            var response = await _coinbaseApiConnector.GetAccounts();

            if (!response.Success)
            {
                return null;
            }

            var coinbaseAccounts = response.Data;
            
            _logger.LogInformation($"Got {coinbaseAccounts.Count()} accounts from Coinbase API");

            return coinbaseAccounts;
        }
    }
}