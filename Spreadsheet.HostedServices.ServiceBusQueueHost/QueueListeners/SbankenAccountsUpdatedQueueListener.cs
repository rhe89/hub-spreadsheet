using Hub.HostedServices.Commands.Logging.Core;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Logging;
using UpdateSbankenAccountsCommand = Spreadsheet.HostedServices.ServiceBusQueueHost.Commands.UpdateSbankenAccountsCommand;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.QueueListeners
{
    public class SbankenAccountsUpdatedQueueListener : ServiceBusHostedService
    {
        public SbankenAccountsUpdatedQueueListener(ILogger<SbankenAccountsUpdatedQueueListener> logger,
            ICommandLogFactory commandLogFactory,
            UpdateSbankenAccountsCommand queuedCommand,
            IQueueProcessor queueProcessor) : base(logger, 
                                                    commandLogFactory, 
                                                    queuedCommand, 
                                                    queueProcessor)
        {
        }

    }
}