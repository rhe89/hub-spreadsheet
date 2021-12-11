using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.QueueListeners;

public class BillingAccountsTransactionsUpdatedService : ServiceBusHostedService
{
    public BillingAccountsTransactionsUpdatedService(ILogger<BillingAccountsTransactionsUpdatedService> logger, 
        IConfiguration configuration,
        UpdateBillingAccountTransactionsCommand queuedCommand, 
        IQueueProcessor queueProcessor,
        TelemetryClient telemetryClient) : base(logger, 
        configuration,
        queuedCommand, 
        queueProcessor,
        telemetryClient)
    {
    }
}