using Hub.Web.Startup;
using Microsoft.Extensions.Configuration;
using Spreadsheet.Data;

namespace Spreadsheet.Web.Api
{
    public class Startup : ApiStartup<SpreadsheetDbContext, DependencyRegistrationFactory>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}