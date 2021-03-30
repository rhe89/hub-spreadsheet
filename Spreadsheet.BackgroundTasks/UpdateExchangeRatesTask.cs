using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateExchangeRatesTask : UpdateTabTaskBase<ExchangeRatesTab>
    {
        public UpdateExchangeRatesTask(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider, 
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            ITabDataProvider<ExchangeRatesTab> tabDataProvider,
            ITabWriterService<ExchangeRatesTab> tabWriterService) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory, tabDataProvider, tabWriterService)
        {
        }
    }
}