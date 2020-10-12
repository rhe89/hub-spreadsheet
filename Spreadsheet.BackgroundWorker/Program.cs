using Microsoft.Extensions.Hosting;

namespace Spreadsheet.BackgroundWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new SpreadsheetWorkerHostBuilder(args).Build().Run();
        }
    }
}