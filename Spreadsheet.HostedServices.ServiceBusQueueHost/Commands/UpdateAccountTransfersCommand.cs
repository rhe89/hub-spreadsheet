using System.Threading;
using System.Threading.Tasks;
using Hub.HostedServices.ServiceBusQueue.Commands;
using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateAccountTransfersCommand : ServiceBusQueueCommand
    {
        public UpdateAccountTransfersCommand(ITabReaderService<ResultsAndSavingsTab> tabReaderService)
        {
        }
        public override Task Execute(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override string QueueName => QueueNames.AccountTransfersUpdated;
    }
}