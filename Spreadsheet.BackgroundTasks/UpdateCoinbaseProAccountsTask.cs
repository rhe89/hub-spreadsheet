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
    public class UpdateCoinbaseProAccountsTask : BackgroundTask
    {
        private readonly ILogger<UpdateCoinbaseAccountsTask> _logger;
        private readonly ICoinbaseProApiConnector _coinbaseProApiConnector;
        private readonly IBankAccountsBalanceTabWriter<CoinbaseProAccountsTabDto> _coinbaseProAccountsBalanceTabWriter;

        public UpdateCoinbaseProAccountsTask(ILogger<UpdateCoinbaseAccountsTask> logger, 
            ICoinbaseProApiConnector coinbaseProApiConnector,
            IBankAccountsBalanceTabWriter<CoinbaseProAccountsTabDto> coinbaseProAccountsBalanceTabWriter, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _logger = logger;
            _coinbaseProApiConnector = coinbaseProApiConnector;
            _coinbaseProAccountsBalanceTabWriter = coinbaseProAccountsBalanceTabWriter;
        }
        public override async Task Execute(CancellationToken cancellationToken)
        {
            var accounts = await GetCoinbaseAccounts();

            if (accounts == null)
            {
                return;
            }
            
            await _coinbaseProAccountsBalanceTabWriter.UpdateTab(accounts);
        }

        private async Task<IList<AccountDto>> GetCoinbaseAccounts()
        {
            _logger.LogInformation("Getting accounts from Coinbase Pro API");

            var response = await _coinbaseProApiConnector.GetAccounts();

            if (!response.Success)
            {
                return null;
            }

            var coinbaseAccounts = response.Data;
            
            _logger.LogInformation($"Got {coinbaseAccounts.Count()} accounts from Coinbase Pro API");

            return coinbaseAccounts;
        }
    }
}