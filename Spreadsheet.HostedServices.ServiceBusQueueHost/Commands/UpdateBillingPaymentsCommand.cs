using Hub.Shared.Storage.ServiceBus;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateBillingPaymentsCommand : UpdateTabCommandBase<BillingPaymentsTab>
{
    public UpdateBillingPaymentsCommand(ITabWriterService<BillingPaymentsTab> tabWriterService,
        ITabDataProvider<BillingPaymentsTab> billingAccountPaymentsDataProvider) : base(billingAccountPaymentsDataProvider, tabWriterService)
    {
    }
        
    public override string Trigger => QueueNames.BankingTransactionsUpdated;
}