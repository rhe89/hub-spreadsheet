using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Extensions;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Services;

namespace Spreadsheet.Services
{
    public class TabWriterService<TTab> : ITabWriterService<TTab>
        where TTab : Tab, new()
    {
        private readonly IGoogleSpreadsheetConnector _googleSpreadsheetConnector;
        private readonly ITabReaderService<TTab> _tabReaderService;
        private readonly ILogger<TabWriterService<TTab>> _logger;

        public TabWriterService(ITabReaderService<TTab> tabReaderService,
                                IGoogleSpreadsheetConnector googleSpreadsheetConnector,
                                ILogger<TabWriterService<TTab>> logger)
        {
            _tabReaderService = tabReaderService;
            _googleSpreadsheetConnector = googleSpreadsheetConnector;
            _logger = logger;
        }

        public async Task UpdateTab(IEnumerable<Cell> rows, DateTime? additionalTimeStamp = null)
        {
            var tab = await _tabReaderService.GetTab();
            
            var columnIndexForCurrentPeriod = tab.GetColumnOfCurrentPeriodInSheet();

            if (columnIndexForCurrentPeriod == -1)
            {
                AddNewColumn(tab, rows, additionalTimeStamp);
            }
            else
            {
                ReplaceColumn(tab, rows, columnIndexForCurrentPeriod, additionalTimeStamp);
            }

            _logger.LogInformation($"Updating {tab.Name}");

            await UpdateTab(tab, tab.Rows.Count);

            _logger.LogInformation($"Finished updating {tab.Name}");
        }
        
        private async Task UpdateTab(Tab tab, int lastRow)
        {
            await _googleSpreadsheetConnector.UpdateSpreadsheetTab(tab, 1, lastRow);
        }

        private void AddNewColumn(Tab tab,
            IEnumerable<Cell> rows, 
            DateTime? additionalTimeStamp)
        {
            var newPeriod = tab.GetCurrentPeriod();

            tab.Rows.First().Cells.Add(newPeriod);

            var rowIndex = 0;

            foreach (var cell in rows)
            {
                var shouldUpdateRow = ShouldUpdateRow(tab, cell, out var accountIndex);

                if (!shouldUpdateRow)
                {
                    continue;
                }
                
                if (accountIndex > rowIndex)
                    rowIndex = accountIndex;

                tab.Rows[accountIndex].Cells.Add(cell.CellValue);
            }

            var lastUpdated = DateTime.Now.FormattedDate();

            tab.Rows[++rowIndex].Cells.Add(lastUpdated);
            
            if (additionalTimeStamp != null)
            {
                tab.Rows[++rowIndex].Cells.Add(additionalTimeStamp.Value.FormattedDate());
            }
        }

        private void ReplaceColumn(Tab tab,
            IEnumerable<Cell> rows,
            int columnIndex, 
            DateTime? additionalTimeStamp)
        {
            var currentRowIndex = 0;
            
            foreach (var cell in rows)
            {
                var shouldUpdateRow = ShouldUpdateRow(tab, cell, out var rowIndex);

                if (!shouldUpdateRow)
                {
                    continue;
                }

                ExpandRowAndCellsIfNecessary(tab, rowIndex, columnIndex);

                tab.Rows[rowIndex].Cells[columnIndex] = cell.CellValue;

                if (rowIndex > currentRowIndex)
                    currentRowIndex = rowIndex;
            }

            SetLastUpdated(tab, columnIndex, ++currentRowIndex);

            if (additionalTimeStamp != null)
            {
                SetAdditionalTimeStamp(tab, additionalTimeStamp.Value, columnIndex,  ++currentRowIndex);
            }
        }

        private static void SetLastUpdated(Tab tab, 
            int columnIndex,
            int rowIndex)
        {
            var lastUpdated = DateTime.Now.FormattedDate();

            var row = tab.Rows[rowIndex];

            if (row.Cells.Count() <= columnIndex)
            {
                row.Cells.Add(lastUpdated);
            }
            else
            {
                row.Cells[columnIndex] = lastUpdated;
            }
        }
        
        private static void SetAdditionalTimeStamp(Tab budgetSpreadsheetTab,
            DateTime additionalTimeStamp,
            int columnIndex, 
            int rowIndex)
        {
            var row = budgetSpreadsheetTab.Rows[rowIndex];

            if (row.Cells.Count() <= columnIndex)
            {
                row.Cells.Add(additionalTimeStamp.FormattedDate());
            }
            else
            {
                row.Cells[columnIndex] = additionalTimeStamp.FormattedDate();;
            }
        }

        private bool ShouldUpdateRow(Tab tab, Cell row, out int rowIndex)
        {
            var rowInTab = tab.Rows.FirstOrDefault(x => x.RowKey == row.RowKey);

            if (rowInTab == null)
            {
                _logger.LogWarning($"No row found in tab {tab.Name} for incoming row {row.RowKey}. Skipping");
                rowIndex = -1;
                return false;
            }

            rowIndex = rowInTab.RowIndex;
            
            return true;
        }

        private static void ExpandRowAndCellsIfNecessary(Tab tab, int currentRowIndex, int currentColumnIndex)
        {
            if (tab.Rows.Count <= currentRowIndex)
            {
                tab.AddRowToExistingSheet(new Row(2));
            }

            while (tab.Rows[currentRowIndex].Cells.Count <= currentColumnIndex)
            {
                tab.Rows[currentRowIndex].Cells.Add(0);
            }
        }
    }
}