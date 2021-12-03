using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;

namespace Spreadsheet.Providers
{
    public class ExchangeRatesTabDataProvider : ITabDataProvider<ExchangeRatesTab>
    {
        private readonly ICoinbaseApiConnector _coinbaseApiConnector;
        private readonly ILogger<ExchangeRatesTabDataProvider> _logger;

        public ExchangeRatesTabDataProvider(ICoinbaseApiConnector coinbaseApiConnector,
            ILogger<ExchangeRatesTabDataProvider> logger)
        {
            _coinbaseApiConnector = coinbaseApiConnector;
            _logger = logger;
        }
        
        public async Task<IEnumerable<Cell>> GetData()
        {
            _logger.LogInformation($"Getting exchange rates from {_coinbaseApiConnector.FriendlyApiName}");

            var response = await _coinbaseApiConnector.GetExchangeRates();

            if (!response.Success)
            {
                throw new ApiConnectorException(response.ErrorMessage);
            }

            var exchangeRates = response.Data;
            
            _logger.LogInformation($"Got {exchangeRates.Count} exchange rates from {_coinbaseApiConnector.FriendlyApiName}");

            return exchangeRates;
        }
    }
}