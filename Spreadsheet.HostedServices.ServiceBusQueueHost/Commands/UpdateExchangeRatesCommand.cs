using Spreadsheet.Shared.Constants;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateExchangeRatesCommand : UpdateTabCommandBase<ExchangeRatesTab>
{
    public UpdateExchangeRatesCommand(ITabDataProvider<ExchangeRatesTab> tabDataProvider,
        ITabWriterService<ExchangeRatesTab> tabWriterService) : base(tabDataProvider, tabWriterService)
    {
    }

    public override string Trigger => QueueNames.ExchangeRatesUpdated;
}