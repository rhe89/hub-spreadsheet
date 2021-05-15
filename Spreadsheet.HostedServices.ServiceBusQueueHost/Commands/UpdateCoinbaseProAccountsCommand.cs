using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateCoinbaseProAccountsCommand : UpdateTabCommandBase<CoinbaseProAccountsTab>
    {
        public UpdateCoinbaseProAccountsCommand(ITabWriterService<CoinbaseProAccountsTab> tabWriterService,
            ITabDataProvider<CoinbaseProAccountsTab> tabDataProvider) : base(tabDataProvider, tabWriterService)
        {
        }

        public override string QueueName => QueueNames.CoinbaseProAccountsUpdated;
    }
}