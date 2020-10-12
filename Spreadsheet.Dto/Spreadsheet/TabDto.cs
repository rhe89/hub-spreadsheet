using System.Collections.Generic;
using System.Linq;
using Spreadsheet.Shared.Constants;

namespace Spreadsheet.Dto.Spreadsheet
{
    public class TabDto
    {
        public string SpreadsheetId { get; set; }
        public string Name { get; set; }
        public string FirstColumn { get; set; }
        public string LastColumn { get; set; }
        public bool PopulateAllRows { get; set; } 
        public IList<RowDto> Rows { get; }
        public IList<RowDto> NewRows { get; }
        public IList<SpreadsheetRowMetadataDto> SpreadsheetRowMetadataDtos { get; set; }

        public TabDto()
        {
            Rows = new List<RowDto>();
            NewRows = new List<RowDto>();
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
                    Rows.Add(new RowDto(i, rowKey, cellsInRow));
            }
        }

        public virtual void AddRow(RowDto row)
        {
            NewRows.Add(row);
        }

        public virtual void AddRowToExistingSheet(RowDto row)
        {
            Rows.Add(row);
        }

        public int GetRowCount()
        {
            return Rows.Count;
        }
    }
}