using System.Net.Http;
using Spreadsheet.Core.Integration;

namespace Spreadsheet.Integration
{
    public class CoinbaseProApiConnector : BankApiConnector, ICoinbaseProApiConnector
    {
        private const string AccountsPath = "/api/account/accounts";
        
        public CoinbaseProApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseProApi") {}
    }
}