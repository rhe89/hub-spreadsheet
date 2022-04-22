using Hub.Shared.Storage.ServiceBus;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateCryptoAccountsCommand : UpdateTabCommandBase<CryptoAccountsTab>
{
    public UpdateCryptoAccountsCommand(ITabWriterService<CryptoAccountsTab> tabWriterService,
        ITabDataProvider<CryptoAccountsTab> tabDataProvider) : base(tabDataProvider, tabWriterService)
    {
    }

    public override string Trigger => QueueNames.CryptoAccountsUpdated;
}