using Spreadsheet.Shared.Constants;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateCoinbaseAccountsCommand : UpdateTabCommandBase<CoinbaseAccountsTab>
    {
        public UpdateCoinbaseAccountsCommand(ITabWriterService<CoinbaseAccountsTab> tabWriterService,
            ITabDataProvider<CoinbaseAccountsTab> tabDataProvider) : base(tabDataProvider, tabWriterService)
        {
        }

        public override string Trigger => QueueNames.CoinbaseAccountsUpdated;
    }
}