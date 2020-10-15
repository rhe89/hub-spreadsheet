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
        private readonly IApiDataTabReader _apiDataTabReader;
        private readonly ILogger<ApiDataTabWriter> _logger;
        
        public ApiDataTabWriter(IApiDataTabReader apiDataTabReader,
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
                                  IList<AccountDto> accounts)
        {
            var newPeriod = BudgetSpreadsheetTabDto.GetCurrentPeriod();

            budgetSpreadsheetTabDto.Rows.First().Cells.Add(newPeriod);

            foreach (var account in accounts)
            {
                var accountIdx = budgetSpreadsheetTabDto.Rows.FirstOrDefault(x => x.RowKey == account.Name)?.RowIndex;

                if (accountIdx == null)
                {
                    _logger.LogWarning($"No row found in tab {budgetSpreadsheetTabDto.Name} for account {account.Name}. Skipping");
                    continue;
                }

                if (account.Balance == 0)
                {
                    _logger.LogWarning($"Available amount in {account.Name} was 0. Skipping.");
                    continue;
                }

                budgetSpreadsheetTabDto.Rows[accountIdx.Value].Cells.Add(account.Balance.ToCommaString());
            }

            var lastUpdated = DateTime.Now.FormattedDateFull();

            budgetSpreadsheetTabDto.Rows.Last().Cells.Add(lastUpdated);
        }

        private void ReplaceCurrentPeriod(BudgetSpreadsheetTabDto budgetSpreadsheetTabDto,
                                          IList<AccountDto> accounts,
                                          int currentPeriodIdx)
        {
            foreach (var account in accounts)
            {
                var accountIdx = budgetSpreadsheetTabDto.Rows.FirstOrDefault(x => x.RowKey == account.Name)?.RowIndex;

                if (accountIdx == null)
                {
                    _logger.LogWarning($"No row found in tab {budgetSpreadsheetTabDto.Name} for account {account.Name}. Skipping");
                    continue;
                }

                if (account.Balance == 0)
                {
                    _logger.LogWarning($"Available amount in {account.Name} was 0. Skipping.");
                    continue;
                }

                ExpandRowAndCellsIfNecessary(budgetSpreadsheetTabDto, accountIdx.Value, currentPeriodIdx);

                budgetSpreadsheetTabDto.Rows[accountIdx.Value].Cells[currentPeriodIdx] = account.Balance.ToCommaString();
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

        private static void ExpandRowAndCellsIfNecessary(TabDto tabDto, int currentRowCount, int currentCellsInRowCount)
        {
            if (tabDto.Rows.Count <= currentRowCount)
                tabDto.AddRowToExistingSheet(new RowDto(2));

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
