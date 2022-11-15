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
    public Row Month => Rows.First();
    public Row LastUpdated => Rows.Last();
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
        if (sheet == null)
        {
            InitializeNewTab();
            FillUnCompleteRows();
            return;
        }
        
        foreach (var cellsInRow in sheet)
        {
            var row = new Row(cellsInRow);

            Rows.Add(row);
        }

        FillUnCompleteRows();
    }

    private void InitializeNewTab()
    {
        //First row = Month row
        var firstRow = new Row(new List<object> { "" });
        
        for (var month = 1; month < NumberOfCellsInRows; month++)
        {
            firstRow.Cells.Add($"{month}/{DateTime.Now.Year}");
        }
        
        Rows.Add(firstRow);
        
        //Last row = Last updated row
        Rows.Add(new Row(new List<object> { "Last updated" }));
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

    public Row AddRow(string rowKey)
    {
        //Shift LastUpdated one row down
        Rows.Add(new Row(LastUpdated.Cells));

        var newRow = Rows[^2];
        
        newRow.Cells = new List<object>(NumberOfCellsInRows) { rowKey };
        
        FillUnCompleteCellsInRow(newRow);

        return newRow;
    }
}