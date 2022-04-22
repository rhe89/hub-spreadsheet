using Hub.Shared.Storage.ServiceBus;
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
}