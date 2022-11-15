using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hub.Shared.Storage.ServiceBus;
using Hub.Shared.Storage.ServiceBus.MessageBody;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateBankingAccountsCommand : UpdateTabCommandBase<BankingAccountsTab>
{
    public UpdateBankingAccountsCommand(ITabWriterService<BankingAccountsTab> tabWriterService,
        ITabDataProvider<BankingAccountsTab> tabDataProvider) : base(tabDataProvider, tabWriterService)
    {
    }

    public override string Trigger => QueueNames.BankingAccountsUpdated;
    
    public override async Task Execute(CancellationToken cancellationToken)
    {
        var data = await TabDataProvider.GetData(Message);
        
        var list = data?.ToList();
            
        if (list == null)
        {
            return;
        }
            
        var bankingAccountBalancesUpdatedBody = BankingAccountBalancesUpdatedBody.Deserialize(Message);
        
        if (bankingAccountBalancesUpdatedBody == null)
        {
            await TabWriterService.UpdateTab(list);
        }
        else
        {
            await TabWriterService.UpdateTab(
                list,
                $"{bankingAccountBalancesUpdatedBody.Month}/{bankingAccountBalancesUpdatedBody.Year}");
        }
    }
}