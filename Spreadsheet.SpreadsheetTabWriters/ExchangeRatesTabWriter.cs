using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Extensions;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.SpreadsheetTabReaders;
using Spreadsheet.Core.SpreadsheetTabWriters;

namespace Spreadsheet.SpreadsheetTabWriters
{
    public class ExchangeRatesTabWriter : TabWriterBase, IExchangeRatesTabWriter
    {
        private readonly ITabReader<ExchangeRatesTabDto> _exchangeRatesTabReader;
        private readonly ILogger<ExchangeRatesTabWriter> _logger;

        public ExchangeRatesTabWriter(IGoogleSpreadsheetConnector googleSpreadsheetConnector,
            ITabReader<ExchangeRatesTabDto> exchangeRatesTabReader,
            ILogger<ExchangeRatesTabWriter> logger) : base(googleSpreadsheetConnector)
        {
            _exchangeRatesTabReader = exchangeRatesTabReader;
            _logger = logger;
        }

        public async Task UpdateTab(IList<ExchangeRateDto> accounts, DateTime bankAccountTaskLastRun)
        {
            var apiDataTabDto = await _exchangeRatesTabReader.GetTab();
            
            var currentPeriodIdx = apiDataTabDto.GetColIndexOfCurrentPeriodInSheet();

            if (currentPeriodIdx == -1)
            {
                AddNewPeriod(apiDataTabDto, accounts, bankAccountTaskLastRun);
            }
            else
            {
                ReplaceCurrentPeriod(apiDataTabDto, accounts, currentPeriodIdx, bankAccountTaskLastRun);
            }

            _logger.LogInformation($"Updating {apiDataTabDto.Name}");

            await UpdateBudgetSheet(apiDataTabDto, apiDataTabDto.Rows.Count);

            _logger.LogInformation($"Finished updating {apiDataTabDto.Name}");
        }

        private void AddNewPeriod(BudgetSpreadsheetTabDtoBase budgetSpreadsheetTabDtoBase,
            IEnumerable<ExchangeRateDto> exchangeRates, DateTime bankAccountTaskLastRun)
        {
            var newPeriod = budgetSpreadsheetTabDtoBase.GetCurrentPeriod();

            budgetSpreadsheetTabDtoBase.Rows.First().Cells.Add(newPeriod);

            var rowIdx = 0;

            foreach (var exchangeRate in exchangeRates)
            {
                var shouldUpdateExchangeRate = ShouldUpdateExchangeRate(budgetSpreadsheetTabDtoBase, exchangeRate, out var accountIdx);

                if (!shouldUpdateExchangeRate)
                {
                    continue;
                }
                
                if (accountIdx > rowIdx)
                    rowIdx = accountIdx;

                budgetSpreadsheetTabDtoBase.Rows[accountIdx].Cells.Add(exchangeRate.NOKRate.ToCommaString());
            }

            var lastUpdated = DateTime.Now.FormattedDateFull();

            budgetSpreadsheetTabDtoBase.Rows[++rowIdx].Cells.Add(lastUpdated);
            budgetSpreadsheetTabDtoBase.Rows[++rowIdx].Cells.Add(bankAccountTaskLastRun.FormattedDateFull());
        }

        private void ReplaceCurrentPeriod(BudgetSpreadsheetTabDtoBase budgetSpreadsheetTabDtoBase,
            IEnumerable<ExchangeRateDto> exchangeRates,
            int currentPeriodIdx, DateTime bankAccountTaskLastRun)
        {
            var rowIdx = 0;
            
            foreach (var account in exchangeRates)
            {
                var shouldUpdateAccount = ShouldUpdateExchangeRate(budgetSpreadsheetTabDtoBase, account, out var accountIdx);

                if (!shouldUpdateAccount)
                {
                    continue;
                }

                ExpandRowAndCellsIfNecessary(budgetSpreadsheetTabDtoBase, accountIdx, currentPeriodIdx);

                budgetSpreadsheetTabDtoBase.Rows[accountIdx].Cells[currentPeriodIdx] = account.NOKRate.ToCommaString();

                if (accountIdx > rowIdx)
                    rowIdx = accountIdx;
            }

            SetLastRun(budgetSpreadsheetTabDtoBase, currentPeriodIdx, ++rowIdx);
        }

        private static void SetLastRun(BudgetSpreadsheetTabDtoBase budgetSpreadsheetTabDtoBase, int currentPeriodIdx,
            int rowIdx)
        {
            var lastUpdated = DateTime.Now.FormattedDateFull();

            var row = budgetSpreadsheetTabDtoBase.Rows[rowIdx];

            if (row.Cells.Count() <= currentPeriodIdx)
            {
                row.Cells.Add(lastUpdated);
            }
            else
            {
                row.Cells[currentPeriodIdx] = lastUpdated;
            }
        }

        private bool ShouldUpdateExchangeRate(TabDtoBase tabDtoBase, ExchangeRateDto account, out int accountIdx)
        {
            var row = tabDtoBase.Rows.FirstOrDefault(x => x.RowKey == account.Currency);

            if (row == null)
            {
                _logger.LogWarning($"No row found in tab {tabDtoBase.Name} for currency {account.Currency}. Skipping");
                accountIdx = -1;
                return false;
            }

            accountIdx = row.RowIndex;
            
            return true;
        }

        private static void ExpandRowAndCellsIfNecessary(TabDtoBase tabDtoBase, int currentRowCount, int currentCellsInRowCount)
        {
            if (tabDtoBase.Rows.Count <= currentRowCount)
            {
                tabDtoBase.AddRowToExistingSheet(new RowDto(2));
            }

            while (tabDtoBase.Rows[currentRowCount].Cells.Count <= currentCellsInRowCount)
            {
                tabDtoBase.Rows[currentRowCount].Cells.Add(0);
            }
        }

        private async Task UpdateBudgetSheet(TabDtoBase tabDtoBase, int lastRow)
        {
            await UpdateSheet(tabDtoBase, 1, lastRow);
        }
        
    }
}