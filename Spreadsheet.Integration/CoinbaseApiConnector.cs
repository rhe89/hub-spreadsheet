using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Shared.Web.Http;
using Spreadsheet.Integration.Dto;

namespace Spreadsheet.Integration
{
    public interface ICoinbaseApiConnector : IBankApiConnector
    {
        Task<Response<IList<ExchangeRateDto>>> GetExchangeRates();
    }
    
    public class CoinbaseApiConnector : BankApiConnector, ICoinbaseApiConnector
    {
        private const string ExchangeRatesPath = "/api/exchangerates";

        public CoinbaseApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseApi") {}
        
        public async Task<Response<IList<ExchangeRateDto>>> GetExchangeRates()
        {
            return await Get<IList<ExchangeRateDto>>(ExchangeRatesPath);
        }
    }
}