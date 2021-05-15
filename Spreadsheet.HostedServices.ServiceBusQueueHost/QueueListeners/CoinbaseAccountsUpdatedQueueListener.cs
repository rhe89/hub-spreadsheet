using Hub.HostedServices.Commands.Logging.Core;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Logging;
using Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.QueueListeners
{
    public class CoinbaseAccountsUpdatedQueueListener : ServiceBusHostedService
    {
        public CoinbaseAccountsUpdatedQueueListener(ILogger<CoinbaseAccountsUpdatedQueueListener> logger, 
            ICommandLogFactory commandLogFactory, 
            UpdateCoinbaseAccountsCommand queuedCommand, 
            IQueueProcessor queueProcessor) : base(logger, 
                                                    commandLogFactory, 
                                                    queuedCommand, 
                                                    queueProcessor)
        {
        }
    }
}