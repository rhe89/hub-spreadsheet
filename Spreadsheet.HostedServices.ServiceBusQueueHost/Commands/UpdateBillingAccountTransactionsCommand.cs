using Spreadsheet.Shared.Constants;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateBillingAccountTransactionsCommand : UpdateTabCommandBase<BillingAccountTab>
{
    public UpdateBillingAccountTransactionsCommand(ITabWriterService<BillingAccountTab> tabWriterService,
        ITabDataProvider<BillingAccountTab> billingAccountPaymentsDataProvider) : base(billingAccountPaymentsDataProvider, tabWriterService)
    {
    }
        
    public override string Trigger => QueueNames.SbankenTransactionsUpdated;
}