using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hub.Shared.DataContracts.Banking.Constants;
using Hub.Shared.Storage.Repository.Core;
using Microsoft.Extensions.Logging;
using Spreadsheet.Shared.Constants;
using Spreadsheet.Data.Dto;
using Spreadsheet.Data.Entities;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Integration.Dto.Spreadsheet.Budget.Tabs;

namespace Spreadsheet.Providers;

public class BillingPaymentsTabDataProvider : ITabDataProvider<BillingPaymentsTab>
{
    private readonly IBankingApiConnector _bankingApiConnector;
    private readonly ISpreadsheetMetadataProvider _spreadsheetMetadataProvider;
    private readonly IHubDbRepository _hubDbRepository;
    private readonly ILogger<BillingPaymentsTabDataProvider> _logger;

    public BillingPaymentsTabDataProvider(IBankingApiConnector bankingApiConnector,
        ISpreadsheetMetadataProvider spreadsheetMetadataProvider,
        IHubDbRepository hubDbRepository,
        ILogger<BillingPaymentsTabDataProvider> logger)
    {
        _bankingApiConnector = bankingApiConnector;
        _spreadsheetMetadataProvider = spreadsheetMetadataProvider;
        _hubDbRepository = hubDbRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ICell>> GetData()
    {
        _logger.LogInformation("Getting row metadata for tab {TabName}",
            SpreadsheetTabMetadataConstants.BillingPaymentsTabName);

        var rows = await _spreadsheetMetadataProvider.GetRowsInTabForCurrentSpreadsheet(SpreadsheetTabMetadataConstants.BillingPaymentsTabName);

        if (rows == null)
        {
            _logger.LogWarning("No metadata for rows in tab {TabName} found", SpreadsheetTabMetadataConstants.BillingPaymentsTabName);
            return null;
        }

        _logger.LogInformation("Got metadata on {Count} rows", rows.Count);

        var transactionsFromBankingApi = await GetBillingPaymentsFromBankingApi();

        if (transactionsFromBankingApi == null) return null;

        Expression<Func<BillingAccountTransaction, bool>> transactionFilter = x => x.TransactionDate.Month == DateTime.Now.Month &&
                                                             x.TransactionDate.Year == DateTime.Now.Year;

        var transactionsInDb = await _hubDbRepository
            .WhereAsync<BillingAccountTransaction, BillingAccountTransactionDto>(transactionFilter);

        var transactionsFromBankingApiList = transactionsFromBankingApi.ToList();

        _logger.LogInformation("Got {Count} transactions to update", transactionsFromBankingApiList.Count);

        var transactionsToUpdateInTab = new List<TransactionCell>();

        var anyNewTransactions = false;

        foreach (var transaction in transactionsFromBankingApiList)
        {
            if (transaction.TransactionId == null)
            {
                continue;
            }

            var rowForTransaction = rows.FirstOrDefault(row =>
                row.TagList.Any(tag => transaction.Description.ToLower().Contains(tag.ToLower())));

            if (rowForTransaction == null)
            {
                _logger.LogInformation(
                    "No corresponding row for transaction with description {Description} found",
                    transaction.Description);

                continue;
            }

            var existingTransactionInDb =
                transactionsInDb.FirstOrDefault(x => x.TransactionId == transaction.TransactionId);

            if (existingTransactionInDb != null)
            {
                continue;
            }

            var newTransaction = new BillingAccountTransactionDto
            {
                Amount = transaction.Amount,
                TransactionId = transaction.TransactionId,
                TransactionDate = transaction.TransactionDate,
                Key = rowForTransaction.RowKey
            };

            _hubDbRepository.QueueAdd<BillingAccountTransaction, BillingAccountTransactionDto>(newTransaction);

            anyNewTransactions = true;
        }

        if (anyNewTransactions)
        {
            await _hubDbRepository.ExecuteQueueAsync();

            transactionsInDb = await _hubDbRepository
                .WhereAsync<BillingAccountTransaction, BillingAccountTransactionDto>(transactionFilter);
        }

        foreach (var row in rows)
        {
            var transactionsForRow = transactionsInDb
                .Where(x => x.Key == row.RowKey)
                .ToList();

            if (!transactionsForRow.Any())
            {
                continue;
            }

            transactionsToUpdateInTab.Add(new TransactionCell
            {
                RowKey = row.RowKey,
                TransactionDate = transactionsForRow.Last().TransactionDate,
                Amount = transactionsForRow.Sum(x => x.Amount)
            });
        }

        _logger.LogInformation("{Count} transactions to update in tab", transactionsToUpdateInTab.Count);

        return transactionsToUpdateInTab;
    }

    private async Task<IEnumerable<TransactionCell>> GetBillingPaymentsFromBankingApi()
    {
        _logger.LogInformation("Getting transactions from {ApiName}", _bankingApiConnector.FriendlyApiName);

        var ageInDays = DateTime.Now.Day;

        var transactions = await _bankingApiConnector.GetTransactions(AccountTypes.Billing, ageInDays);

        _logger.LogInformation("Got {Count} transactions from {ApiName}", transactions.Count, _bankingApiConnector.FriendlyApiName);

        return transactions;
    }
}