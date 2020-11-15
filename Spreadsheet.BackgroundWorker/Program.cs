using Spreadsheet.Data;
using Hub.HostedServices.Timer;
using Microsoft.Extensions.Hosting;

namespace Spreadsheet.BackgroundWorker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new BackgroundWorker<DependencyRegistrationFactory, SpreadsheetDbContext>(args, "SQL_DB_SPREADSHEET")
                .CreateHostBuilder()
                .Build()
                .Run();
        }
    }
}