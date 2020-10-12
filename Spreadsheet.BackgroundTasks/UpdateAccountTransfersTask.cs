using System.Threading;
using System.Threading.Tasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Factories;
using Hub.Storage.Providers;
using Spreadsheet.SpreadsheetTabReaders;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateAccountTransfersTask : BackgroundTask
    {
        private IResultOgSavingsTabReader _resultOgSavingsTabReader;

        public UpdateAccountTransfersTask(IResultOgSavingsTabReader resultOgSavingsTabReader, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _resultOgSavingsTabReader = resultOgSavingsTabReader;
        }
        public override Task Execute(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}