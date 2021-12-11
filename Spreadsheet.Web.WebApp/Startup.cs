using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Spreadsheet.Web.WebApp;

[UsedImplicitly]
public class Startup : Hub.Shared.Web.BlazorServer.Startup<DependencyRegistrationFactory>
{
    public Startup(IConfiguration configuration) : base(configuration)
    {
    }
}