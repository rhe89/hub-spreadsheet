using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Web.Http;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Integration;

namespace Spreadsheet.Integration
{
    public class CoinbaseApiConnector : BankApiConnector, ICoinbaseApiConnector
    {
        private const string ExchangeRatesPath = "/api/exchangerates/exchangerates";

        public CoinbaseApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseApi") {}
        
        public async Task<Response<IList<ExchangeRateDto>>> GetExchangeRates()
        {
            return await Get<IList<ExchangeRateDto>>(ExchangeRatesPath);
        }

        
    }
}