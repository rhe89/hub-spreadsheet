using Hub.Web.HostBuilder;
using Microsoft.Extensions.Hosting;
using Spreadsheet.Data;

namespace Spreadsheet.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string [] args)
        {
            return ApiHostBuilder.CreateHostBuilder<Startup, DependencyRegistrationFactory, SpreadsheetDbContext>(args);
        }
    }
}