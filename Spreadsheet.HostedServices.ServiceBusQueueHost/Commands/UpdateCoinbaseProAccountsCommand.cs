using Spreadsheet.Shared.Constants;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateCoinbaseProAccountsCommand : UpdateTabCommandBase<CoinbaseProAccountsTab>
    {
        public UpdateCoinbaseProAccountsCommand(ITabWriterService<CoinbaseProAccountsTab> tabWriterService,
            ITabDataProvider<CoinbaseProAccountsTab> tabDataProvider) : base(tabDataProvider, tabWriterService)
        {
        }

        public override string Trigger => QueueNames.CoinbaseProAccountsUpdated;
    }
}