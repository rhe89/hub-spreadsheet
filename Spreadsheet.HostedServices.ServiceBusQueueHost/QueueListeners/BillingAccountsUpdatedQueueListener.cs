using Hub.HostedServices.Commands.Logging.Core;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.QueueListeners
{
    public class BillingAccountsTransactionsUpdatedService : ServiceBusHostedService
    {
        public BillingAccountsTransactionsUpdatedService(ILogger<BillingAccountsTransactionsUpdatedService> logger, 
            ICommandLogFactory commandLogFactory, 
            IConfiguration configuration,
            UpdateBillingAccountTransactionsCommand queuedCommand, 
            IQueueProcessor queueProcessor) : base(logger, 
                                                 commandLogFactory, 
                                                 configuration,
                                                 queuedCommand, 
                                                 queueProcessor)
        {
        }
    }
}