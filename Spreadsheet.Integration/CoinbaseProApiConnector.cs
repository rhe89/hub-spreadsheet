using System.Net.Http;

namespace Spreadsheet.Integration
{
    public interface ICoinbaseProApiConnector : IBankApiConnector
    {
    }
    
    public class CoinbaseProApiConnector : BankApiConnector, ICoinbaseProApiConnector
    {
        public CoinbaseProApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseProApi") {}
    }
}