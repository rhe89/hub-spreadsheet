using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateExchangeRatesCommand : UpdateTabCommandBase<ExchangeRatesTab>
    {
        public UpdateExchangeRatesCommand(ITabDataProvider<ExchangeRatesTab> tabDataProvider,
            ITabWriterService<ExchangeRatesTab> tabWriterService) : base(tabDataProvider, tabWriterService)
        {
        }

        public override string QueueName => QueueNames.ExchangeRatesUpdated;
    }
}