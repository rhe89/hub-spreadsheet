using System.Net.Http;
using Spreadsheet.Core.Integration;

namespace Spreadsheet.Integration
{
    public class CoinbaseApiConnector : BankApiConnector, ICoinbaseApiConnector
    {
        public CoinbaseApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseApi") {}
    }
}