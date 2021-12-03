using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.QueueListeners
{
    public class CoinbaseAccountsUpdatedQueueListener : ServiceBusHostedService
    {
        public CoinbaseAccountsUpdatedQueueListener(ILogger<CoinbaseAccountsUpdatedQueueListener> logger, 
            IConfiguration configuration,
            UpdateCoinbaseAccountsCommand queuedCommand, 
            IQueueProcessor queueProcessor,
            TelemetryClient telemetryClient) : base(logger, 
                                                 configuration,
                                                 queuedCommand, 
                                                 queueProcessor,
                                                 telemetryClient)
        {
        }
    }
}