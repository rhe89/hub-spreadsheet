using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

public class BillingAccountTransactionsProvider : ITabDataProvider<BillingAccountTab>
{
    private readonly ISbankenApiConnector _sbankenApiConnector;
    private readonly ISpreadsheetMetadataProvider _spreadsheetMetadataProvider;
    private readonly IHubDbRepository _hubDbRepository;
    private readonly ILogger<BillingAccountTransactionsProvider> _logger;

    public BillingAccountTransactionsProvider(ISbankenApiConnector sbankenApiConnector,
        ISpreadsheetMetadataProvider spreadsheetMetadataProvider,
        IHubDbRepository hubDbRepository,
        ILogger<BillingAccountTransactionsProvider> logger)
    {
        _sbankenApiConnector = sbankenApiConnector;
        _spreadsheetMetadataProvider = spreadsheetMetadataProvider;
        _hubDbRepository = hubDbRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ICell>> GetData()
    {
        _logger.LogInformation("Getting row metadata for tab {TabName}",
            SpreadsheetTabMetadataConstants.BillingAccountTabName);

        var rows = await _spreadsheetMetadataProvider.GetRowsInTabForCurrentSpreadsheet(SpreadsheetTabMetadataConstants.BillingAccountTabName);

        if (rows == null)
        {
            _logger.LogWarning("No metadata for rows in tab {TabName} found", SpreadsheetTabMetadataConstants.BillingAccountTabName);
            return null;
        }

        _logger.LogInformation("Got metadata on {Count} rows", rows.Count);

        var transactionsFromSbanken = await GetBillingAccountTransactionsFromSbankenApi();

        if (transactionsFromSbanken == null) return null;

        static Expression<Func<BillingAccountTransaction, bool>> Predicate()
        {
            return x => x.TransactionDate.Month == DateTime.Now.Month &&
                        x.TransactionDate.Year == DateTime.Now.Year;
        }

        var transactionsInDb = await _hubDbRepository
            .WhereAsync<BillingAccountTransaction, BillingAccountTransactionDto>(Predicate());

        var transactionsFromSbankenList = transactionsFromSbanken.ToList();

        _logger.LogInformation("Got {Count} transactions to update from Sbanken", transactionsFromSbankenList.Count);

        var transactionsToUpdateInTab = new List<TransactionDto>();

        var anyNewTransactions = false;

        foreach (var transaction in transactionsFromSbankenList)
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
                .WhereAsync<BillingAccountTransaction, BillingAccountTransactionDto>(Predicate());
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

            transactionsToUpdateInTab.Add(new TransactionDto
            {
                RowKey = row.RowKey,
                TransactionDate = transactionsForRow.Last().TransactionDate,
                Amount = transactionsForRow.Sum(x => x.Amount)
            });
        }

        _logger.LogInformation("{Count} transactions to update in tab", transactionsToUpdateInTab.Count);

        return transactionsToUpdateInTab;
    }

    private async Task<IEnumerable<TransactionDto>> GetBillingAccountTransactionsFromSbankenApi()
    {
        _logger.LogInformation("Getting transactions from {ApiName}", _sbankenApiConnector.FriendlyApiName);

        var ageInDays = DateTime.Now.Day;

        var transactions = await _sbankenApiConnector.GetBillingAccountTransactions(ageInDays);

        _logger.LogInformation("Got {Count} transactions from {ApiName}", transactions.Count,
            _sbankenApiConnector.FriendlyApiName);

        return transactions;
    }
}