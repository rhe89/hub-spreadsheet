using Spreadsheet.Shared.Constants;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateSbankenAccountsCommand : UpdateTabCommandBase<SbankenAccountsTab>
    {
        public UpdateSbankenAccountsCommand(ITabWriterService<SbankenAccountsTab> tabWriterService,
            ITabDataProvider<SbankenAccountsTab> tabDataProvider) : base(tabDataProvider, tabWriterService)
        {
        }

        public override string Trigger => QueueNames.SbankenAccountsUpdated;
    }
}