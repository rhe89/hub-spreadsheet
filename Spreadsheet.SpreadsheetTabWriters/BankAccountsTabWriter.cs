using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spreadsheet.Core.Dto.BackgroundTasks;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Extensions;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.SpreadsheetTabReaders;
using Spreadsheet.Core.SpreadsheetTabWriters;

namespace Spreadsheet.SpreadsheetTabWriters
{
    public class BankAccountsTabWriter<TBankAccountsTabDto> : TabWriterBase, IBankAccountsTabWriter<TBankAccountsTabDto>
        where TBankAccountsTabDto : BankAccountsTabDto, new()
    {
        private readonly ITabReader<TBankAccountsTabDto> _apiDataTabReader;
        private readonly ILogger<BankAccountsTabWriter<TBankAccountsTabDto>> _logger;
        
        public BankAccountsTabWriter(ITabReader<TBankAccountsTabDto> apiDataTabReader,
                                IGoogleSpreadsheetConnector googleSpreadsheetConnector,
                                ILogger<BankAccountsTabWriter<TBankAccountsTabDto>> logger) : base(googleSpreadsheetConnector)
        {
            _apiDataTabReader = apiDataTabReader;
            _logger = logger;
        }

        public async Task UpdateTab(IList<AccountDto> accounts, DateTime bankAccountTaskLastRun)
        {
            var apiDataTabDto = await _apiDataTabReader.GetTab();
            
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
            IEnumerable<AccountDto> accounts, DateTime bankAccountTaskLastRun)
        {
            var newPeriod = BudgetSpreadsheetTabDtoBase.GetCurrentPeriod();

            budgetSpreadsheetTabDtoBase.Rows.First().Cells.Add(newPeriod);

            var rowIdx = 0;

            foreach (var account in accounts)
            {
                var shouldUpdateAccount = ShouldUpdateAccount(budgetSpreadsheetTabDtoBase, account, out var accountIdx);

                if (!shouldUpdateAccount)
                {
                    continue;
                }
                
                if (accountIdx > rowIdx)
                    rowIdx = accountIdx;

                budgetSpreadsheetTabDtoBase.Rows[accountIdx].Cells.Add(account.Balance.ToCommaString());
            }

            var lastUpdated = DateTime.Now.FormattedDateFull();

            budgetSpreadsheetTabDtoBase.Rows[++rowIdx].Cells.Add(lastUpdated);
            budgetSpreadsheetTabDtoBase.Rows[++rowIdx].Cells.Add(bankAccountTaskLastRun.FormattedDateFull());
        }

        private void ReplaceCurrentPeriod(BudgetSpreadsheetTabDtoBase budgetSpreadsheetTabDtoBase,
            IEnumerable<AccountDto> accounts,
            int currentPeriodIdx, DateTime bankAccountTaskLastRun)
        {
            var rowIdx = 0;
            
            foreach (var account in accounts)
            {
                var shouldUpdateAccount = ShouldUpdateAccount(budgetSpreadsheetTabDtoBase, account, out var accountIdx);

                if (!shouldUpdateAccount)
                {
                    continue;
                }

                ExpandRowAndCellsIfNecessary(budgetSpreadsheetTabDtoBase, accountIdx, currentPeriodIdx);

                budgetSpreadsheetTabDtoBase.Rows[accountIdx].Cells[currentPeriodIdx] = account.Balance.ToCommaString();

                if (accountIdx > rowIdx)
                    rowIdx = accountIdx;
            }

            SetLastRun(budgetSpreadsheetTabDtoBase, currentPeriodIdx, ++rowIdx);
            SetBankAccountTaskLastRun(budgetSpreadsheetTabDtoBase, currentPeriodIdx, bankAccountTaskLastRun, ++rowIdx);
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
        
        private static void SetBankAccountTaskLastRun(BudgetSpreadsheetTabDtoBase budgetSpreadsheetTabDtoBase,
            int currentPeriodIdx, DateTime bankAccountTaskLastRun, int rowIdx)
        {
            var row = budgetSpreadsheetTabDtoBase.Rows[rowIdx];

            if (row.Cells.Count() <= currentPeriodIdx)
            {
                row.Cells.Add(bankAccountTaskLastRun.FormattedDateFull());
            }
            else
            {
                row.Cells[currentPeriodIdx] = bankAccountTaskLastRun.FormattedDateFull();;
            }
        }

        private bool ShouldUpdateAccount(TabDtoBase tabDtoBase, AccountDto account, out int accountIdx)
        {
            var row = tabDtoBase.Rows.FirstOrDefault(x => x.RowKey == account.Name);

            if (row == null)
            {
                _logger.LogWarning($"No row found in tab {tabDtoBase.Name} for account {account.Name}. Skipping");
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
