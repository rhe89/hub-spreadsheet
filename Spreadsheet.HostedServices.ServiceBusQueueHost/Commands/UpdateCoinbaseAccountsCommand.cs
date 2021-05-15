using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateCoinbaseAccountsCommand : UpdateTabCommandBase<CoinbaseAccountsTab>
    {
        public UpdateCoinbaseAccountsCommand(ITabWriterService<CoinbaseAccountsTab> tabWriterService,
            ITabDataProvider<CoinbaseAccountsTab> tabDataProvider) : base(tabDataProvider, tabWriterService)
        {
        }

        public override string QueueName => QueueNames.CoinbaseAccountsUpdated;
    }
}