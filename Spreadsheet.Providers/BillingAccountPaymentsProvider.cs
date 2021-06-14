using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hub.Storage.Repository.Core;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Data;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Entities;
using Spreadsheet.Core.Exceptions;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Providers;

namespace Spreadsheet.Providers
{
    public class BillingAccountPaymentsProvider : ITabDataProvider<BillingAccountTab>
    {
        private readonly ISbankenApiConnector _sbankenApiConnector;
        private readonly ISpreadsheetProvider _spreadsheetProvider;
        private readonly IHubDbRepository _hubDbRepository;
        private readonly ILogger<BillingAccountPaymentsProvider> _logger;

        public BillingAccountPaymentsProvider(ISbankenApiConnector sbankenApiConnector,
            ISpreadsheetProvider spreadsheetProvider,
            IHubDbRepository hubDbRepository,
            ILogger<BillingAccountPaymentsProvider> logger)
        {
            _sbankenApiConnector = sbankenApiConnector;
            _spreadsheetProvider = spreadsheetProvider;
            _hubDbRepository = hubDbRepository;
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
            
            var transactionsFromSbanken = await GetBillingAccountPaymentsFromSbankenApi();

            if (transactionsFromSbanken == null)
            {
                return null;
            }

            var paymentsFromSbanken = transactionsFromSbanken.Where(x => x.Amount < 0).ToList();
            
            static Expression<Func<BillingAccountPayment, bool>> Predicate() => 
                x => x.TransactionDate.Month == DateTime.Now.Month &&
                     x.TransactionDate.Year == DateTime.Now.Year;

            var paymentsInDb = await _hubDbRepository
                .WhereAsync<BillingAccountPayment, BillingAccountPaymentDto>(Predicate());
            
            _logger.LogInformation($"Got {paymentsFromSbanken.Count} payments to update");
            
            var paymentsToUpdateInTab = new List<TransactionDto>();
            
            foreach (var payment in paymentsFromSbanken)
            {
                if (payment.TransactionId == null)
                {
                    continue;
                }
                
                var rowForPayment = rows.FirstOrDefault(row =>
                    row.TagList.Any(tag => payment.Description.ToLower().Contains(tag.ToLower())));
                
                if (rowForPayment == null)
                {
                    _logger.LogInformation($"No corresponding row for payment with description {payment.Description} found");
                    
                    continue;
                }

                var existingPaymentInDb = paymentsInDb.FirstOrDefault(x => x.TransactionId == payment.TransactionId);

                if (existingPaymentInDb != null)
                {
                    continue;
                }

                var newPayment = new BillingAccountPaymentDto
                {
                    Amount = payment.Amount,
                    TransactionId = payment.TransactionId,
                    TransactionDate = payment.TransactionDate,
                    Key = rowForPayment.RowKey
                };
                
                _hubDbRepository.QueueAdd<BillingAccountPayment, BillingAccountPaymentDto>(newPayment);
                
                var previousPaymentsInSameCategoryInDb =
                    paymentsInDb.Where(x => x.Key == rowForPayment.RowKey);

                paymentsToUpdateInTab.Add(new TransactionDto
                {
                    RowKey = rowForPayment.RowKey,
                    TransactionDate = payment.TransactionDate,
                    Amount = decimal.Negate(payment.Amount) + previousPaymentsInSameCategoryInDb.Sum(x => x.Amount),
                });
            }

            await _hubDbRepository.ExecuteQueueAsync();
            
            _logger.LogInformation($"Found {paymentsToUpdateInTab.Count} payments to update in tab");
            
            return paymentsToUpdateInTab;
        }
        
        private async Task<IEnumerable<TransactionDto>> GetBillingAccountPaymentsFromSbankenApi()
        {
            _logger.LogInformation($"Getting transactions from {_sbankenApiConnector.FriendlyApiName}");

            var ageInDays = DateTime.Now.Day;
            
            var response = await _sbankenApiConnector.GetBillingAccountTransactions(ageInDays);

            if (!response.Success)
            {
                throw new ApiConnectorException(response.ErrorMessage);
            }

            var transactions = response.Data;
            
            _logger.LogInformation($"Got {transactions.Count} transactions from {_sbankenApiConnector.FriendlyApiName}");

            return transactions;
        }
    }
}