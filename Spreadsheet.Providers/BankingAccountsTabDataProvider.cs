using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;

namespace Spreadsheet.Providers;

public class BankingAccountsTabDataProvider : ITabDataProvider<BankingAccountsTab>
{
    private readonly IBankingApiConnector _bankingApiConnector;
    private readonly ILogger<BankingAccountsTabDataProvider> _logger;

    public BankingAccountsTabDataProvider(IBankingApiConnector bankingApiConnector,
        ILogger<BankingAccountsTabDataProvider> logger)
    {
        _bankingApiConnector = bankingApiConnector;
        _logger = logger;
    }

    public async Task<IEnumerable<ICell>> GetData()
    {
        _logger.LogInformation("Getting accounts from {ApiName}", _bankingApiConnector.FriendlyApiName);

        var accounts = await _bankingApiConnector.GetAccounts();

        _logger.LogInformation("Got {Count} accounts from {ApiName}", accounts.Count, _bankingApiConnector.FriendlyApiName);

        return accounts;
    }
}