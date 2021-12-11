using System.IO;
using Hub.Shared.Web.BlazorServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Spreadsheet.Web.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            HostBuilder<Startup, DependencyRegistrationFactory>.Create(args);
    }
}