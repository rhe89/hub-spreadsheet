using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.QueueListeners
{
    public class SbankenAccountsUpdatedQueueListener : ServiceBusHostedService
    {
        public SbankenAccountsUpdatedQueueListener(ILogger<SbankenAccountsUpdatedQueueListener> logger,
            IConfiguration configuration,
            UpdateSbankenAccountsCommand queuedCommand,
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