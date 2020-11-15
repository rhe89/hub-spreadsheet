using System.Collections.Generic;

namespace Spreadsheet.Core.Dto.Spreadsheet
{
    public class RowDto
    {
        public int RowIndex { get; set; }
        public string RowKey { get; set; }
        public IList<object> Cells { get; set; }
        
        public RowDto(int rowIndex, string rowKey, IList<object> cells)
        {
            RowIndex = rowIndex;
            RowKey = rowKey;
            Cells = cells;
        }

        public RowDto(int columns)
        {
            Cells = new List<object>();

            for (var i = 0; i <= columns; i++)
            {
                Cells.Add(new object());
            }
        }

    }
}