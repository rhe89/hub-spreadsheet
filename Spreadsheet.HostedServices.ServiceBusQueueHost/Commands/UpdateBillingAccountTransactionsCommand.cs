using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateBillingAccountTransactionsCommand : UpdateTabCommandBase<BillingAccountTab>
    {
        public UpdateBillingAccountTransactionsCommand(ITabWriterService<BillingAccountTab> tabWriterService,
            ITabDataProvider<BillingAccountTab> billingAccountPaymentsDataProvider) : base(billingAccountPaymentsDataProvider, tabWriterService)
        {
        }
        
        public override string QueueName => QueueNames.SbankenTransactionsUpdated;
    }
}