using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.QueueListeners;

public class CryptoAccountsUpdatedQueueListener : ServiceBusHostedService
{
    public CryptoAccountsUpdatedQueueListener(ILogger<CryptoAccountsUpdatedQueueListener> logger, 
        IConfiguration configuration,
        UpdateCryptoAccountsCommand queuedCommand, 
        IQueueProcessor queueProcessor,
        TelemetryClient telemetryClient) : base(logger, 
        configuration,
        queuedCommand, 
        queueProcessor,
        telemetryClient)
    {
    }

}