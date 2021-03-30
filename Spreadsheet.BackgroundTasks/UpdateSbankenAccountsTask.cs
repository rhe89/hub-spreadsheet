using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateSbankenAccountsTask : UpdateTabTaskBase<SbankenAccountsTab>
    {
        public UpdateSbankenAccountsTask(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider, 
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            ITabWriterService<SbankenAccountsTab> tabWriterService,
            ITabDataProvider<SbankenAccountsTab> tabDataProvider) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory, tabDataProvider, tabWriterService)
        {
        }
    }
}