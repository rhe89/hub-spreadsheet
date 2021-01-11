using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.SpreadsheetTabWriters;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateCoinbaseProAccountsTask : UpdateBankAccountTaskBase<CoinbaseProAccountsTabDto>
    {
        public UpdateCoinbaseProAccountsTask(ILogger<UpdateCoinbaseProAccountsTask> logger, 
            ICoinbaseProApiConnector coinbaseProApiConnector,
            IBankAccountsTabWriter<CoinbaseProAccountsTabDto> coinbaseProAccountsTabWriter, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : 
                base(backgroundTaskConfigurationProvider, 
                     backgroundTaskConfigurationFactory, 
                     coinbaseProAccountsTabWriter,
                     coinbaseProApiConnector,
                     logger)
        { }
       
    }
}