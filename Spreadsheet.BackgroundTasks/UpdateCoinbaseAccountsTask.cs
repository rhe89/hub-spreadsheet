using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateCoinbaseAccountsTask : UpdateTabTaskBase<CoinbaseAccountsTab>
    {
        public UpdateCoinbaseAccountsTask(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider, 
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            ITabWriterService<CoinbaseAccountsTab> tabWriterService,
            ITabDataProvider<CoinbaseAccountsTab> tabDataProvider) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory, tabDataProvider, tabWriterService)
        {
        }
    }
}