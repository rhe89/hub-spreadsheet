using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hub.Shared.DataContracts.Banking.Query;
using Hub.Shared.Storage.ServiceBus.MessageBody;
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

    public async Task<IEnumerable<ICell>> GetData(string parameters)
    {
        var bankingAccountsUpdatedBody = BankingAccountBalancesUpdatedBody.Deserialize(parameters);
        
        var month = bankingAccountsUpdatedBody?.Month ?? DateTime.Now.Month;
        var year = bankingAccountsUpdatedBody?.Year ?? DateTime.Now.Year;

        var accountQuery = new AccountQuery
        {
            IncludeDiscontinuedAccounts = true,
            IncludeSharedAccounts = true,
            BalanceToDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
        };
        
        _logger.LogInformation(
            "Getting account balances from {ApiName} for {Month}.{Year}",
            _bankingApiConnector.FriendlyApiName,
            month,
            year);
        
        var accounts = await _bankingApiConnector.GetAccountBalances(accountQuery);

        _logger.LogInformation("Got {Count} accounts from {ApiName}", accounts.Count, _bankingApiConnector.FriendlyApiName);

        return accounts.OrderBy(x => x.CreatedDate);
    }
}