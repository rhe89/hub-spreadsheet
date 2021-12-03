using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.QueueListeners
{
    public class ExchangeRatesUpdatedService : ServiceBusHostedService
    {
        public ExchangeRatesUpdatedService(ILogger<ExchangeRatesUpdatedService> logger,
            IConfiguration configuration,
            UpdateExchangeRatesCommand queuedCommand,
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