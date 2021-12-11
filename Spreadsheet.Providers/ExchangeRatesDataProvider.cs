using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;

namespace Spreadsheet.Providers;

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

    public async Task<IEnumerable<ICell>> GetData()
    {
        _logger.LogInformation("Getting exchange rates from {ApiName}", _coinbaseApiConnector.FriendlyApiName);

        var exchangeRates = await _coinbaseApiConnector.GetExchangeRates();

        _logger.LogInformation("Got {Count} exchange rates from {FriendlyApiName}", exchangeRates.Count,
            _coinbaseApiConnector.FriendlyApiName);

        return exchangeRates;
    }
}