using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;

namespace Spreadsheet.Providers;

public class CryptoAccountsTabDataProvider : ITabDataProvider<CryptoAccountsTab>
{
    private readonly ICryptoApiConnector _cryptoAccountApiConnector;
    private readonly ILogger<CryptoAccountsTabDataProvider> _logger;

    public CryptoAccountsTabDataProvider(ICryptoApiConnector cryptoAccountApiConnector,
        ILogger<CryptoAccountsTabDataProvider> logger)
    {
        _cryptoAccountApiConnector = cryptoAccountApiConnector;
        _logger = logger;
    }

    public async Task<IEnumerable<ICell>> GetData()
    {
        _logger.LogInformation("Getting accounts from {ApiName}", _cryptoAccountApiConnector.FriendlyApiName);

        var accounts = await _cryptoAccountApiConnector.GetAccounts();

        _logger.LogInformation("Got {Count} accounts from {ApiName}", accounts.Count, _cryptoAccountApiConnector.FriendlyApiName);

        return accounts;
    }
}