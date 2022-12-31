using Hub.Shared.HostedServices.ServiceBusQueue;
using Microsoft.Extensions.Hosting;
using Spreadsheet.Data;

ServiceBusHostBuilder
    .CreateHostBuilder(args)
    .Build()
    .Run();