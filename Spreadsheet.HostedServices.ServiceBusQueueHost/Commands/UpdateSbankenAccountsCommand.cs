using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateSbankenAccountsCommand : UpdateTabCommandBase<SbankenAccountsTab>
    {
        public UpdateSbankenAccountsCommand(ITabWriterService<SbankenAccountsTab> tabWriterService,
            ITabDataProvider<SbankenAccountsTab> tabDataProvider) : base(tabDataProvider, tabWriterService)
        {
        }

        public override string QueueName => QueueNames.SbankenAccountsUpdated;
    }
}