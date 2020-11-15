using System.Threading;
using System.Threading.Tasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.SpreadsheetTabReaders;
namespace Spreadsheet.BackgroundTasks
{
    public class UpdateAccountTransfersTask : BackgroundTask
    {
        private readonly ITabReader<ResultsAndSavingsTabDto> _tabReader;

        public UpdateAccountTransfersTask(ITabReader<ResultsAndSavingsTabDto> tabReader, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _tabReader = tabReader;
        }
        public override Task Execute(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}