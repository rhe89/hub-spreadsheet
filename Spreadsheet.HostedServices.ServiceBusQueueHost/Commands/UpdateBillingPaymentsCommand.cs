using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hub.Shared.Storage.ServiceBus;
using Hub.Shared.Storage.ServiceBus.MessageBody;
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
    
    public override async Task Execute(CancellationToken cancellationToken)
    {
        var data = await TabDataProvider.GetData(Message);
        
        var list = data?.ToList();
            
        if (list == null)
        {
            return;
        }
            
        var bankingTransactionsUpdatedBody = BankingTransactionsUpdatedBody.Deserialize(Message);
        
        if (bankingTransactionsUpdatedBody == null)
        {
            await TabWriterService.UpdateTab(list);
        }
        else
        {
            await TabWriterService.UpdateTab(list, $"{bankingTransactionsUpdatedBody.Month}/{bankingTransactionsUpdatedBody.Year}");
        }
    }
}