using Hub.Shared.Web.Api;
using Microsoft.Extensions.Hosting;
using Spreadsheet.Data;

namespace Spreadsheet.Web.Api;

public static class Program
{
    public static void Main(string[] args)
    { 
        CreateHostBuilder(args)
            .Build()
            .Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return HostBuilder<DependencyRegistrationFactory, SpreadsheetDbContext>
            .Create(args);
    }
}