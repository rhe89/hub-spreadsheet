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
    public class ApiDataTabWriter : TabWriterBase, IApiDataTabWriter
    {
        private readonly ITabReader<ApiDataTabDto> _apiDataTabReader;
        private readonly ILogger<ApiDataTabWriter> _logger;
        
        public ApiDataTabWriter(ITabReader<ApiDataTabDto> apiDataTabReader,
                                IGoogleSpreadsheetConnector googleSpreadsheetConnector,
                                ILogger<ApiDataTabWriter> logger) : base(googleSpreadsheetConnector)
        {
            _apiDataTabReader = apiDataTabReader;
            _logger = logger;
        }

        public async Task UpdateTab(AccountDto accountDto)
        {
            await UpdateTab(new List<AccountDto> {accountDto});
        }
        
        public async Task UpdateTab(IList<AccountDto> accounts)
        {
            var apiDataTabDto = await _apiDataTabReader.GetTab();
            
            var currentPeriodIdx = apiDataTabDto.GetColIndexOfCurrentPeriodInSheet();

            if (currentPeriodIdx == -1)
            {
                AddNewPeriod(apiDataTabDto, accounts);
            }
            else
            {
                ReplaceCurrentPeriod(apiDataTabDto, accounts, currentPeriodIdx);
            }

            _logger.LogInformation($"Updating {apiDataTabDto.Name}");

            await UpdateBudgetSheet(apiDataTabDto, apiDataTabDto.Rows.Count);

            _logger.LogInformation($"Finished updating {apiDataTabDto.Name}");
        }

        private void AddNewPeriod(BudgetSpreadsheetTabDtoBase budgetSpreadsheetTabDtoBase,
                                  IEnumerable<AccountDto> accounts)
        {
            var newPeriod = BudgetSpreadsheetTabDtoBase.GetCurrentPeriod();

            budgetSpreadsheetTabDtoBase.Rows.First().Cells.Add(newPeriod);

            foreach (var account in accounts)
            {
                var shouldUpdateAccount = ShouldUpdateAccount(budgetSpreadsheetTabDtoBase, account, out var accountIdx);

                if (!shouldUpdateAccount)
                {
                    continue;
                }

                budgetSpreadsheetTabDtoBase.Rows[accountIdx].Cells.Add(account.Balance.ToCommaString());
            }

            var lastUpdated = DateTime.Now.FormattedDateFull();

            budgetSpreadsheetTabDtoBase.Rows.Last().Cells.Add(lastUpdated);
        }

        private void ReplaceCurrentPeriod(BudgetSpreadsheetTabDtoBase budgetSpreadsheetTabDtoBase,
                                          IEnumerable<AccountDto> accounts,
                                          int currentPeriodIdx)
        {
            foreach (var account in accounts)
            {
                var shouldUpdateAccount = ShouldUpdateAccount(budgetSpreadsheetTabDtoBase, account, out var accountIdx);

                if (!shouldUpdateAccount)
                {
                    continue;
                }

                ExpandRowAndCellsIfNecessary(budgetSpreadsheetTabDtoBase, accountIdx, currentPeriodIdx);

                budgetSpreadsheetTabDtoBase.Rows[accountIdx].Cells[currentPeriodIdx] = account.Balance.ToCommaString();
            }

            var lastUpdated = DateTime.Now.FormattedDateFull();

            var lastRow = budgetSpreadsheetTabDtoBase.Rows.Last();

            if (lastRow.Cells.Count() <= currentPeriodIdx)
            {
                lastRow.Cells.Add(lastUpdated);
            }
            else
            {
                lastRow.Cells[currentPeriodIdx] = lastUpdated;
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

            if (account.Balance == 0)
            {
                _logger.LogWarning($"Available amount in {account.Name} was 0. Skipping.");
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
