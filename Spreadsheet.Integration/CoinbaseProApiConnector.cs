using System.Net.Http;
using Spreadsheet.Core.Integration;

namespace Spreadsheet.Integration
{
    public class CoinbaseProApiConnector : BankApiConnector, ICoinbaseProApiConnector
    {
        public CoinbaseProApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseProApi") {}
    }
}