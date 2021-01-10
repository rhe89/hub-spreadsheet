using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Web.Http;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.BackgroundTasks;
using Spreadsheet.Core.Integration;

namespace Spreadsheet.Integration
{
    public class CoinbaseProApiConnector : HttpClientService, ICoinbaseProApiConnector
    {
        private const string AccountsPath = "/api/account/accounts";
        
        public CoinbaseProApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseProApi") {}
        
        public async Task<Response<IList<AccountDto>>> GetAccounts()
        {
            return await Get<IList<AccountDto>>(AccountsPath);
        }
    }
}