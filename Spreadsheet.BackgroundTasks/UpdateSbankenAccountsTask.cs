using System.Collections.Generic;
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
    public class UpdateSbankenAccountsTask : BackgroundTask
    {
        private readonly ILogger<UpdateSbankenAccountsTask> _logger;
        private readonly ISbankenApiConnector _sbankenApiConnector;
        private readonly IBankAccountsBalanceTabWriter<SbankenAccountsTabDto> _sbankenAccountsBalanceTabWriter;

        public UpdateSbankenAccountsTask(ILogger<UpdateSbankenAccountsTask> logger, 
            ISbankenApiConnector sbankenApiConnector,
            IBankAccountsBalanceTabWriter<SbankenAccountsTabDto> sbankenAccountsBalanceTabWriter, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _logger = logger;
            _sbankenApiConnector = sbankenApiConnector;
            _sbankenAccountsBalanceTabWriter = sbankenAccountsBalanceTabWriter;
        }
        
        public override async Task Execute(CancellationToken cancellationToken)
        {
            var sbankenAccounts = await GetSbankenAccounts();

            if (sbankenAccounts == null)
            {
                return;
            }

            await _sbankenAccountsBalanceTabWriter.UpdateTab(sbankenAccounts);
        }
        
        private async Task<IList<AccountDto>> GetSbankenAccounts()
        {
            _logger.LogInformation("Getting accounts from Sbanken API");

            var response = await _sbankenApiConnector.GetAccounts();

            if (!response.Success)
            {
                return null;
            }

            var sbankenAccounts = response.Data;

            _logger.LogInformation($"Got {sbankenAccounts.Count} accounts from Sbanken API");
            
            return sbankenAccounts;
        }
    }
}