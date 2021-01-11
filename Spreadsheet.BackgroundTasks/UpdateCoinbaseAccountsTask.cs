using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.SpreadsheetTabWriters;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateCoinbaseAccountsTask : UpdateBankAccountTaskBase<CoinbaseAccountsTabDto>
    {
        public UpdateCoinbaseAccountsTask(ILogger<UpdateCoinbaseAccountsTask> logger, 
            ICoinbaseApiConnector coinbaseApiConnector,
            IBankAccountsTabWriter<CoinbaseAccountsTabDto> coinbaseAccountsTabWriter, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : 
            base(backgroundTaskConfigurationProvider, 
                backgroundTaskConfigurationFactory, 
                coinbaseAccountsTabWriter,
                coinbaseApiConnector,
                logger)
        { }
    }
}