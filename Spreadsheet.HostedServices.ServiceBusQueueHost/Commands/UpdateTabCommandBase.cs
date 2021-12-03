using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands
{
    public abstract class UpdateTabCommandBase<TTab> : ServiceBusQueueCommand
        where TTab : Tab, new()
    {
        private readonly ITabDataProvider<TTab> _tabDataProvider;
        private readonly ITabWriterService<TTab> _tabWriterService;

        protected UpdateTabCommandBase(ITabDataProvider<TTab> tabDataProvider,
            ITabWriterService<TTab> tabWriterService)
        {
            _tabDataProvider = tabDataProvider;
            _tabWriterService = tabWriterService;
        }
        
        public override async Task Execute(CancellationToken cancellationToken)
        {
            var data = await _tabDataProvider.GetData();

            var list = data?.ToList();
            
            if (list == null)
            {
                return;
            }
            
            await _tabWriterService.UpdateTab(list);
        }
    }
}