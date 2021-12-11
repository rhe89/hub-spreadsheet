using Hub.Shared.HostedServices.ServiceBusQueue;
using Microsoft.Extensions.Hosting;
using Spreadsheet.Data;

namespace Spreadsheet.HostedServices.ServiceBusQueueHost;

public static class Program
{
    public static void Main(string[] args)
    {
        new Bootstrapper<DependencyRegistrationFactory, SpreadsheetDbContext>(args, 
                "SQL_DB_SPREADSHEET")
            .CreateHostBuilder()
            .Build()
            .Run();
    }
}