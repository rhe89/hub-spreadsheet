using Hub.HostedServices.Commands.Logging.Core;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Logging;
using Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.QueueListeners
{
    public class CoinbaseProAccountsUpdatedQueueListener : ServiceBusHostedService
    {
        public CoinbaseProAccountsUpdatedQueueListener(ILogger<CoinbaseProAccountsUpdatedQueueListener> logger, 
            ICommandLogFactory commandLogFactory, 
            UpdateCoinbaseProAccountsCommand queuedCommand, 
            IQueueProcessor queueProcessor) : base(logger, 
                                                    commandLogFactory, 
                                                    queuedCommand, 
                                                    queueProcessor)
        {
        }

    }
}