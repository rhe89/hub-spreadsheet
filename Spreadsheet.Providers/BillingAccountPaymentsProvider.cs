using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Providers;

namespace Spreadsheet.Providers
{
    public class BillingAccountPaymentsProvider : ITabDataProvider<BillingAccountTab>
    {
        private readonly ISbankenApiConnector _sbankenApiConnector;
        private readonly ISpreadsheetProvider _spreadsheetProvider;
        private readonly ILogger<BillingAccountPaymentsProvider> _logger;

        public BillingAccountPaymentsProvider(ISbankenApiConnector sbankenApiConnector,
            ISpreadsheetProvider spreadsheetProvider,
            ILogger<BillingAccountPaymentsProvider> logger)
        {
            _sbankenApiConnector = sbankenApiConnector;
            _spreadsheetProvider = spreadsheetProvider;
            _logger = logger;
        }
        
        public async Task<IEnumerable<Cell>> GetData()
        {
            _logger.LogInformation($"Getting row metadata for tab {SpreadsheetTabMetadataConstants.BillingAccountTabName}");

            var rows = await _spreadsheetProvider.GetRowsInTabForCurrentSpreadsheet(SpreadsheetTabMetadataConstants
                .BillingAccountTabName);

            if (rows == null)
            {
                _logger.LogWarning($"No metadata for rows in tab {SpreadsheetTabMetadataConstants.BillingAccountTabName} found");
                return null;
            }
            
            _logger.LogInformation($"Got metadata on {rows.Count} rows");
            
            var transactions = await GetTransactionsFromSbankenApi();

            if (transactions == null)
            {
                return null;
            }

            var payments = transactions.Where(x => x.Amount < 0).ToList();
            
            _logger.LogInformation($"Got {payments.Count} payments to update");
            
            var paymentsToUpdateInTab = new Dictionary<string, TransactionDto>();
            
            foreach (var payment in payments)
            {
                var rowForPayment = rows.FirstOrDefault(row =>
                    row.TagList.Any(tag => payment.Name.ToLower().Contains(tag.ToLower())));

                if (rowForPayment == null)
                {
                    _logger.LogInformation($"No corresponding row for payment with name {payment.Name} found");
                    
                    continue;
                }

                var anotherPaymentForCategoryAlreadyExists =
                    paymentsToUpdateInTab.TryGetValue(rowForPayment.RowKey, out var existingPayment);

                if (anotherPaymentForCategoryAlreadyExists)
                {
                    existingPayment.Amount += decimal.Negate(payment.Amount);
                }
                else
                {
                    paymentsToUpdateInTab.Add(rowForPayment.RowKey, new TransactionDto
                    {
                        Name = rowForPayment.RowKey,
                        TransactionDate = payment.TransactionDate,
                        Amount = decimal.Negate(payment.Amount)
                    });
                }
            }
            
            _logger.LogInformation($"Found {paymentsToUpdateInTab.Count} payments to update in tab");
            
            return paymentsToUpdateInTab.Values;
        }
        
        private async Task<IEnumerable<TransactionDto>> GetTransactionsFromSbankenApi()
        {
            _logger.LogInformation($"Getting transactions from {_sbankenApiConnector.FriendlyApiName}");

            var response = await _sbankenApiConnector.GetBillingAccountTransactions();

            if (!response.Success)
            {
                _logger.LogError(response.ErrorMessage);
                return null;
            }

            var transactions = response.Data;
            
            _logger.LogInformation($"Got {transactions.Count} transactions from {_sbankenApiConnector.FriendlyApiName}");

            return transactions;
        }
    }
}