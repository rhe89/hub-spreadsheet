using Microsoft.Extensions.Logging;
using Spreadsheet.Dto.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spreadsheet.Dto.Spreadsheet;
using Spreadsheet.Integration;
using Spreadsheet.Shared.Extensions;
using Spreadsheet.SpreadsheetTabReaders;

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

        private void AddNewPeriod(BudgetSpreadsheetTabDto budgetSpreadsheetTabDto,
                                  IEnumerable<AccountDto> accounts)
        {
            var newPeriod = BudgetSpreadsheetTabDto.GetCurrentPeriod();

            budgetSpreadsheetTabDto.Rows.First().Cells.Add(newPeriod);

            foreach (var account in accounts)
            {
                var shouldUpdateAccount = ShouldUpdateAccount(budgetSpreadsheetTabDto, account, out var accountIdx);

                if (!shouldUpdateAccount)
                {
                    continue;
                }

                budgetSpreadsheetTabDto.Rows[accountIdx].Cells.Add(account.Balance.ToCommaString());
            }

            var lastUpdated = DateTime.Now.FormattedDateFull();

            budgetSpreadsheetTabDto.Rows.Last().Cells.Add(lastUpdated);
        }

        private void ReplaceCurrentPeriod(BudgetSpreadsheetTabDto budgetSpreadsheetTabDto,
                                          IEnumerable<AccountDto> accounts,
                                          int currentPeriodIdx)
        {
            foreach (var account in accounts)
            {
                var shouldUpdateAccount = ShouldUpdateAccount(budgetSpreadsheetTabDto, account, out var accountIdx);

                if (!shouldUpdateAccount)
                {
                    continue;
                }

                ExpandRowAndCellsIfNecessary(budgetSpreadsheetTabDto, accountIdx, currentPeriodIdx);

                budgetSpreadsheetTabDto.Rows[accountIdx].Cells[currentPeriodIdx] = account.Balance.ToCommaString();
            }

            var lastUpdated = DateTime.Now.FormattedDateFull();

            var lastRow = budgetSpreadsheetTabDto.Rows.Last();

            if (lastRow.Cells.Count() <= currentPeriodIdx)
            {
                lastRow.Cells.Add(lastUpdated);
            }
            else
            {
                lastRow.Cells[currentPeriodIdx] = lastUpdated;
            }
        }

        private bool ShouldUpdateAccount(TabDto tabDto, AccountDto account, out int accountIdx)
        {
            var row = tabDto.Rows.FirstOrDefault(x => x.RowKey == account.Name);

            if (row == null)
            {
                _logger.LogWarning($"No row found in tab {tabDto.Name} for account {account.Name}. Skipping");
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

        private static void ExpandRowAndCellsIfNecessary(TabDto tabDto, int currentRowCount, int currentCellsInRowCount)
        {
            if (tabDto.Rows.Count <= currentRowCount)
            {
                tabDto.AddRowToExistingSheet(new RowDto(2));
            }

            while (tabDto.Rows[currentRowCount].Cells.Count <= currentCellsInRowCount)
            {
                tabDto.Rows[currentRowCount].Cells.Add(0);
            }
        }

        private async Task UpdateBudgetSheet(TabDto tabDto, int lastRow)
        {
            await UpdateSheet(tabDto, 1, lastRow);
        }
    }
}
