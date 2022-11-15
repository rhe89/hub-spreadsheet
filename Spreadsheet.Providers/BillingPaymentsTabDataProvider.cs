using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hub.Shared.DataContracts.Banking.Constants;
using Hub.Shared.DataContracts.Banking.Query;
using Hub.Shared.Storage.ServiceBus.MessageBody;
using Microsoft.Extensions.Logging;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Shared;

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

    public async Task<IEnumerable<ICell>> GetData(string parameters)
    {
        var bankingTransactionsUpdatedBody = BankingTransactionsUpdatedBody.Deserialize(parameters);
        
        var month = bankingTransactionsUpdatedBody?.Month ?? DateTime.Now.Month;
        var year = bankingTransactionsUpdatedBody?.Year ?? DateTime.Now.Year;
        
        _logger.LogInformation(
            "Getting scheduled transactions from {ApiName} for {Month}.{Year}", 
            _bankingApiConnector.FriendlyApiName,
            month,
            year);
        
        var allScheduledTransactions = await _bankingApiConnector.GetScheduledTransactions(new ScheduledTransactionQuery
        {
            IncludeCompletedTransactions = true,
            AccountType = AccountTypes.Billing,
            NextTransactionFromDate = DateTimeUtils.FirstDayOfMonth(year, month),
            NextTransactionToDate = DateTimeUtils.LastDayOfMonth(year, month)
        });

        _logger.LogInformation("Got {Count} scheduled transactions from {ApiName}", allScheduledTransactions.Count, _bankingApiConnector.FriendlyApiName);
        
        var allScheduledBills = allScheduledTransactions.Sum(x => x.Amount);
        
        return new List<ICell>
        {
            new BillingPaymentCell { RowKey = "BUDGETED", CellValue = allScheduledBills.ToString(CultureInfo.InvariantCulture) },
        };
    }
}