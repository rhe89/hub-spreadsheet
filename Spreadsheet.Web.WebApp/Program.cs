using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Spreadsheet.Web.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configPath = $"{Directory.GetCurrentDirectory()}/../..";
            
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(configPath)
                .AddJsonFile("appsettings.json", true)
                .Build();
            
            CreateHostBuilder(args, configuration).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot configuration) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureHostConfiguration(configurationBuilder => configurationBuilder.AddConfiguration(configuration));
    }
}