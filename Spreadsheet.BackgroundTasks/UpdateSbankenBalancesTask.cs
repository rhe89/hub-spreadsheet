using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.BackgroundTasks;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.SpreadsheetTabWriters;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateSbankenBalancesTask : BackgroundTask
    {
        private readonly ILogger<UpdateSbankenBalancesTask> _logger;
        private readonly ISbankenApiConnector _sbankenApiConnector;
        private readonly IApiDataTabWriter _apiDataTabWriter;

        public UpdateSbankenBalancesTask(ILogger<UpdateSbankenBalancesTask> logger, 
            ISbankenApiConnector sbankenApiConnector,
            IApiDataTabWriter apiDataTabWriter, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _logger = logger;
            _sbankenApiConnector = sbankenApiConnector;
            _apiDataTabWriter = apiDataTabWriter;
        }
        
        public override async Task Execute(CancellationToken cancellationToken)
        {
            var sbankenAccounts = await GetSbankenAccounts();

            if (sbankenAccounts == null)
            {
                return;
            }

            await _apiDataTabWriter.UpdateTab(sbankenAccounts);
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

            _logger.LogInformation($"Got {sbankenAccounts.Count()} accounts from Sbanken API");
            
            return sbankenAccounts;
        }
    }
}