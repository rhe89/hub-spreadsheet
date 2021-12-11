using System;
using System.Collections.Generic;
using System.Linq;
using Spreadsheet.Shared.Constants;
using Spreadsheet.Data.Dto;

namespace Spreadsheet.Integration.Dto.Spreadsheet;

public abstract class Tab
{
    public string SpreadsheetId { get; init; }
    public string Name { get; init; }
    public string FirstColumn { get; init; }
    public string LastColumn { get; init; }
    private bool PopulateAllRows { get; } 
    public IList<Row> Rows { get; }
    public IList<SpreadsheetMetadataDto.Row> SpreadsheetRowMetadataDtos { get; init; }

    protected Tab()
    {
        Rows = new List<Row>();
        PopulateAllRows = true;
    }
        
    public int GetColumnOfCurrentPeriodInSheet()
    {
        var currentPeriod = GetCurrentPeriod();

        return GetColIndexOfPeriodInSheet(currentPeriod);
    }
        
    private int GetColIndexOfPeriodInSheet(string period)
    {
        var idx = 0;

        var periodRow = Rows.First();

        foreach (var col in periodRow.Cells)
        {
            if (col.ToString() == period)
            {
                return idx;
            }

            idx++;
        }

        return -1;
    }

    public static string GetCurrentPeriod()
    {
        return $"{DateTime.Now.Month}/{DateTime.Now.Year}";
    }

    public void PopulateRows(IList<IList<object>> sheet)
    {
        for (var i = 0; i < sheet.Count; i++)
        {
            var cellsInRow = sheet[i];
            var rowKey = cellsInRow[0].ToString();
                
            if (PopulateAllRows || 
                i == SpreadsheetRowMetadataConstants.PeriodRowIndex || 
                SpreadsheetRowMetadataDtos.Any(x => x.RowKey == rowKey))
            {
                Rows.Add(new Row(i, rowKey, cellsInRow));
            }
        }
    }

    public void AddRowToExistingSheet(Row row)
    {
        Rows.Add(row);
    }
}