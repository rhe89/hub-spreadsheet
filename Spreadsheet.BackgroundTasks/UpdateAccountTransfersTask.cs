using System.Threading;
using System.Threading.Tasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Services;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateAccountTransfersTask : BackgroundTask
    {
        public UpdateAccountTransfersTask(ITabReaderService<ResultsAndSavingsTab> tabReaderService, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
        }
        public override Task Execute(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}