using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Storage.Core.Dto;
using Hub.Web.Http;
using Spreadsheet.Core.Dto.Integration;

namespace Spreadsheet.Integration
{
    public class BankApiConnector : HttpClientService
    {
        private const string AccountsPath = "/api/account/accounts";
        private const string BackgroundTaskConfigurationsPath = "/api/backgroundtaskconfiguration/backgroundtaskconfigurations";
        
        public BankApiConnector(HttpClient httpClient, string friendlyApiName) : base(httpClient, friendlyApiName) {}
        
        public async Task<Response<IList<AccountDto>>> GetAccounts()
        {
            return await Get<IList<AccountDto>>(AccountsPath);
        }

        public async Task<Response<IList<BackgroundTaskConfigurationDto>>> GetBackgroundTaskConfigurations()
        {
            return await Get<IList<BackgroundTaskConfigurationDto>>(BackgroundTaskConfigurationsPath);
        }
    }
}