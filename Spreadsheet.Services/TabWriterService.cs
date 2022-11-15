using System;
using System.Collections.Generic;
using System.Globalization;
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
    Task UpdateTab(IList<ICell> cellsInColumn);
    Task UpdateTab(IList<ICell> cellsInColumn, string period);
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
    
    public async Task UpdateTab(IList<ICell> cellsInColumn)
    {
        var currentPeriod = Tab.GetCurrentPeriod();

        await UpdateTab(cellsInColumn, currentPeriod);
    }

    public async Task UpdateTab(IList<ICell> cellsInColumn, string period)
    {
        var tab = await _tabReaderService.GetTab();
        
        var columnIndexForCurrentPeriod = tab.GetColIndexOfPeriodInSheet(period);

        var columnExistsForCurrentPeriod = columnIndexForCurrentPeriod != -1;
        
        if (columnExistsForCurrentPeriod)
        {
            UpdateCellsInColumn(tab, cellsInColumn, columnIndexForCurrentPeriod);
        }
        else
        {
            columnIndexForCurrentPeriod = AddNewColumn(tab);
            
            UpdateCellsInColumn(tab, cellsInColumn, columnIndexForCurrentPeriod);
        }

        _logger.LogInformation("Updating {TabName}", tab.Name);

        await UpdateTab(tab, tab.Rows.Count);

        _logger.LogInformation("Finished updating {TabName}", tab.Name);
    }
        
    private async Task UpdateTab(TTab tab, int lastRow)
    {
        await _googleSpreadsheetConnector.UpdateSpreadsheetTab(tab, 1, lastRow);
    }

    private int AddNewColumn(TTab tab)
    {
        var newPeriod = Tab.GetCurrentPeriod();
            
        tab.Month.Cells.Add(newPeriod);

        tab.FillUnCompleteRows();

        return tab.NumberOfCellsInRows;
    }

    private void UpdateCellsInColumn(TTab tab,
        IList<ICell> incomingCellsInColumn,
        int columnIndex)
    {
        var matchingCells = new List<ICell>();

        var preservedRows = new List<Row>{ tab.Month, tab.LastUpdated };
        
        foreach (var row in tab.Rows)
        {
            if (preservedRows.Any(preservedRow => preservedRow == row))
            {
                continue;
            }

            var matchingCell = incomingCellsInColumn
                .FirstOrDefault(incomingCell => incomingCell.RowKey != null &&
                                                (row.RowKey.Contains(incomingCell.RowKey) ||
                                                 (string.IsNullOrEmpty(row.RowKey))));

            if (matchingCell == null)
            {
                continue;
            }
            
            matchingCells.Add(matchingCell);

            row.Cells[columnIndex] = decimal
                .Parse(matchingCell.CellValue, CultureInfo.InvariantCulture)
                .ReplacePeriodWithComma();
        }

        var nonMatchingCells = incomingCellsInColumn.Except(matchingCells);

        foreach (var incomingCell in nonMatchingCells)
        {
            var newRow = tab.AddRow(incomingCell.RowKey);
            
            newRow.Cells[columnIndex] = decimal
                .Parse(incomingCell.CellValue, NumberStyles.Number)
                .ReplacePeriodWithComma();
        }

        tab.LastUpdated.Cells[columnIndex] = DateTime.Now.FormattedDate();
    }
}