using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateCoinbaseProAccountsTask : UpdateTabTaskBase<CoinbaseProAccountsTab>
    {
        public UpdateCoinbaseProAccountsTask(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider, 
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            ITabWriterService<CoinbaseProAccountsTab> tabWriterService,
            ITabDataProvider<CoinbaseProAccountsTab> tabDataProvider) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory, tabDataProvider, tabWriterService)
        {
        }
       
    }
}