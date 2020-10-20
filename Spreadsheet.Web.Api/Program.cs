using Hub.Web.HostBuilder;
using Spreadsheet.Data;

namespace Spreadsheet.Web.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new ApiHostBuilder<Startup, DependencyRegistrationFactory, SpreadsheetDbContext>().CreateHostBuilder(args);        
        }
    }
}