using Hub.Shared.Web.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spreadsheet.Data;

namespace Spreadsheet.Web.Api
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactory<SpreadsheetDbContext>
    {
        public DependencyRegistrationFactory() : base("SQL_DB_SPREADSHEET", "Spreadsheet.Data")
        {
        }

        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
        }
    }
}