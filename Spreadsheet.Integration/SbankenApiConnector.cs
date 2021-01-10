using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Web.Http;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.BackgroundTasks;
using Spreadsheet.Core.Integration;

namespace Spreadsheet.Integration
{
    public class SbankenApiConnector : HttpClientService, ISbankenApiConnector
    {
        private const string AccountsPath = "/api/account/accounts";
        
        public SbankenApiConnector(HttpClient httpClient) : base(httpClient, "SbankenApi") {}
        
        public async Task<Response<IList<AccountDto>>> GetAccounts()
        {
            return await Get<IList<AccountDto>>(AccountsPath);
        }
    }
}