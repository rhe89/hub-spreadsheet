using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;

namespace Spreadsheet.Providers;

public class ExchangeRatesTabDataProvider : ITabDataProvider<ExchangeRatesTab>
{
    private readonly ICryptoApiConnector _cryptoApiConnector;
    private readonly ILogger<ExchangeRatesTabDataProvider> _logger;

    public ExchangeRatesTabDataProvider(ICryptoApiConnector cryptoApiConnector,
        ILogger<ExchangeRatesTabDataProvider> logger)
    {
        _cryptoApiConnector = cryptoApiConnector;
        _logger = logger;
    }

    public async Task<IEnumerable<ICell>> GetData(string parameters)
    {
        _logger.LogInformation("Getting exchange rates from {ApiName}", _cryptoApiConnector.FriendlyApiName);

        var exchangeRates = await _cryptoApiConnector.GetExchangeRates();

        _logger.LogInformation("Got {Count} exchange rates from {FriendlyApiName}", exchangeRates.Count, _cryptoApiConnector.FriendlyApiName);

        return exchangeRates;
    }
}