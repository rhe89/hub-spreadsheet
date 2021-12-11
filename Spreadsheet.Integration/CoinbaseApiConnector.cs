using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Spreadsheet.Integration.Dto;

namespace Spreadsheet.Integration
{
    public interface ICoinbaseApiConnector : IBankApiConnector
    {
        Task<IList<ExchangeRateDto>> GetExchangeRates();
    }

    [UsedImplicitly]
    public class CoinbaseApiConnector : BankApiConnector, ICoinbaseApiConnector
    {
        private const string ExchangeRatesPath = "/api/exchangerates";

        public CoinbaseApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseApi")
        {
        }

        public async Task<IList<ExchangeRateDto>> GetExchangeRates()
        {
            return await Get<IList<ExchangeRateDto>>(ExchangeRatesPath);
        }
    }
}