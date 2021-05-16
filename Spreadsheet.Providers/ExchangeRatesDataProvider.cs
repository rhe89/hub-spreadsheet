using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Exceptions;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Providers;

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