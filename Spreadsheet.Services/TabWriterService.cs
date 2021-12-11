using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Spreadsheet.Shared.Extensions;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Services;

public interface ITabWriterService<[UsedImplicitly]TTab>
    where TTab : Tab, new()
{
    Task UpdateTab(IList<ICell> rows);
}
    
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

    public async Task UpdateTab(IList<ICell> rows)
    {
        var tab = await _tabReaderService.GetTab();
            
        var columnIndexForCurrentPeriod = tab.GetColumnOfCurrentPeriodInSheet();

        if (columnIndexForCurrentPeriod == -1)
        {
            AddNewColumn(tab, rows);
        }
        else
        {
            ReplaceColumn(tab, rows, columnIndexForCurrentPeriod);
        }

        _logger.LogInformation("Updating {TabName}", tab.Name);

        await UpdateTab(tab, tab.Rows.Count);

        _logger.LogInformation("Finished updating {TabName}", tab.Name);
    }
        
    private async Task UpdateTab(TTab tab, int lastRow)
    {
        await _googleSpreadsheetConnector.UpdateSpreadsheetTab(tab, 1, lastRow);
    }

    private void AddNewColumn(TTab tab,
        IList<ICell> incomingCells)
    {
        var newPeriod = Tab.GetCurrentPeriod();

        tab.Rows.First().Cells.Add(newPeriod);
            
        foreach (var row in tab.Rows)
        {
            var shouldUpdateRow = ShouldUpdateRow(tab, incomingCells, row, out var rowIndex);

            if (!shouldUpdateRow)
            {
                continue;
            }
                
            var cellInRowToUpdate = incomingCells.First(x => x.RowKey == row.RowKey);

            tab.Rows[rowIndex].Cells.Add(cellInRowToUpdate.CellValue);
        }

        var lastUpdated = DateTime.Now.FormattedDate();

        tab.Rows[^1].Cells.Add(lastUpdated);
    }

    private void ReplaceColumn(TTab tab,
        IList<ICell> incomingCells,
        int columnIndex)
    {
        foreach (var row in tab.Rows)
        {
            var shouldUpdateRow = ShouldUpdateRow(tab, incomingCells, row, out var rowIndex);

            if (!shouldUpdateRow)
            {
                continue;
            }

            var cellInRowToUpdate = incomingCells.First(x => x.RowKey == row.RowKey);

            ExpandRowAndCellsIfNecessary(tab, rowIndex, columnIndex);

            tab.Rows[rowIndex].Cells[columnIndex] = cellInRowToUpdate.CellValue;
        }

        SetLastUpdated(tab, columnIndex);
    }

    private static void SetLastUpdated(TTab tab, 
        int columnIndex)
    {
        var lastUpdated = DateTime.Now.FormattedDate();

        var row = tab.Rows[^1];

        if (row.Cells.Count <= columnIndex)
        {
            row.Cells.Add(lastUpdated);
        }
        else
        {
            row.Cells[columnIndex] = lastUpdated;
        }
    }
        
    private bool ShouldUpdateRow(TTab tab, IEnumerable<ICell> incomingCells, Row row, out int rowIndex)
    {
        var cellInRowToUpdate = incomingCells.FirstOrDefault(x => x.RowKey == row.RowKey);

        if (cellInRowToUpdate == null)
        {
            _logger.LogInformation("No incoming data (cell value) provided for row {Row} in tab {Tab}", row.RowKey, tab.Name);
            rowIndex = -1;
            return false;
        }

        rowIndex = row.RowIndex;
            
        return true;
    }

    private static void ExpandRowAndCellsIfNecessary(TTab tab, int currentRowIndex, int currentColumnIndex)
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