using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Web.Http;
using Spreadsheet.Core.Dto.Integration;

namespace Spreadsheet.Integration
{
    public abstract class BankApiConnector : HttpClientService
    {
        private const string AccountsPath = "/api/accounts";

        protected BankApiConnector(HttpClient httpClient, string friendlyApiName) : base(httpClient, friendlyApiName) {}
        
        public async Task<Response<IList<AccountDto>>> GetAccounts()
        {
            return await Get<IList<AccountDto>>(AccountsPath);
        }
    }
}