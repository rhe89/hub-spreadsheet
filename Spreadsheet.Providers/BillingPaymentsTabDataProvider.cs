using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hub.Shared.DataContracts.Banking.Constants;
using Microsoft.Extensions.Logging;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;

namespace Spreadsheet.Providers;

public class BillingPaymentsTabDataProvider : ITabDataProvider<BillingPaymentsTab>
{
    private readonly IBankingApiConnector _bankingApiConnector;
    private readonly ILogger<BillingPaymentsTabDataProvider> _logger;

    public BillingPaymentsTabDataProvider(IBankingApiConnector bankingApiConnector,
        ILogger<BillingPaymentsTabDataProvider> logger)
    {
        _bankingApiConnector = bankingApiConnector;
        _logger = logger;
    }

    public async Task<IEnumerable<ICell>> GetData()
    {
        _logger.LogInformation("Getting billing payments from {ApiName}", _bankingApiConnector.FriendlyApiName);

        var ageInDays = DateTime.Now.Day;

        var transactions = await _bankingApiConnector.GetTransactions(AccountTypes.Billing, ageInDays);
        
        _logger.LogInformation("Got {Count} transactions from {ApiName}", transactions.Count, _bankingApiConnector.FriendlyApiName);

        return transactions;
    }
}