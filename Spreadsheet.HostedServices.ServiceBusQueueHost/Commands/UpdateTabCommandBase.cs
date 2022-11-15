using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Providers;
using Spreadsheet.Services;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost.Commands;

public abstract class UpdateTabCommandBase<TTab> : ServiceBusQueueCommand
    where TTab : Tab, new()
{
    protected readonly ITabDataProvider<TTab> TabDataProvider;
    protected readonly ITabWriterService<TTab> TabWriterService;

    protected UpdateTabCommandBase(ITabDataProvider<TTab> tabDataProvider,
        ITabWriterService<TTab> tabWriterService)
    {
        TabDataProvider = tabDataProvider;
        TabWriterService = tabWriterService;
    }
        
    public override async Task Execute(CancellationToken cancellationToken)
    {
        var data = await TabDataProvider.GetData(Message);

        var list = data?.ToList();
            
        if (list == null)
        {
            return;
        }
            
        await TabWriterService.UpdateTab(list);
    }
}