using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Web.Http;
using Microsoft.Extensions.Logging;
using Spreadsheet.Dto.Account;

namespace Spreadsheet.Integration
{
    public class SbankenApiConnector : HttpClientService, ISbankenApiConnector
    {
        private const string AccountsPath = "/api/account/accounts";
        
        public SbankenApiConnector(HttpClient httpClient, ILogger<SbankenApiConnector> logger) : base(httpClient, logger, "SbankenApi") {}
        
        public async Task<Response<IList<AccountDto>>> GetAccounts()
        {
            return await Get<IList<AccountDto>>(AccountsPath);
        }
    }
}