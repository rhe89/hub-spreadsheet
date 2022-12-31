using System;
using System.Collections.Generic;
using System.Linq;

namespace Spreadsheet.Integration.Dto.Spreadsheet;

public abstract class Tab
{
    public string SpreadsheetId { get; init; }
    public string Name { get; init; }
    public string FirstColumn { get; init; }
    public string LastColumn { get; init; }
    public IList<Row> Rows { get; }
    public int NumberOfCellsInRows { get; set; }

    protected Tab()
    {
        Rows = new List<Row>();
    }

    public void Init()
    {
        var indexOfLastColumn = LastColumn.ToUpper()[0] - 64;

        NumberOfCellsInRows = indexOfLastColumn;
    }
        
    public int GetColIndexOfPeriodInSheet(string period)
    {
        var periodRow = Rows.First();

        for (var index = 0; index < periodRow.Cells.Count; index++)
        {
            var col = periodRow.Cells[index];
            
            if (col.ToString() == period)
            {
                return index;
            }
        }

        return -1;
    }

    public static string GetCurrentPeriod()
    {
        return $"{DateTime.Now.Month}/{DateTime.Now.Year}";
    }

    public void PopulateRows(IList<IList<object>> sheet)
    {
        foreach (var cellsInRow in sheet)
        {
            var row = new Row(cellsInRow);

            Rows.Add(row);
        }

        FillUnCompleteRows();
    }

    public void FillUnCompleteRows()
    {
        foreach (var row in Rows)
        {
            FillUnCompleteCellsInRow(row);
        }
    }

    private void FillUnCompleteCellsInRow(Row row)
    {
        for (var cellIndex = 0; cellIndex < NumberOfCellsInRows; cellIndex++)
        {
            if (row.Cells.ElementAtOrDefault(cellIndex) == null)
            {
                row.Cells.Insert(cellIndex, "");
            }
        }
    }
}