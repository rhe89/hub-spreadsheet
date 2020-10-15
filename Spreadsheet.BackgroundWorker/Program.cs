using Microsoft.Extensions.Hosting;

namespace Spreadsheet.BackgroundWorker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new SpreadsheetWorkerHostBuilder(args).Build().Run();
        }
    }
}