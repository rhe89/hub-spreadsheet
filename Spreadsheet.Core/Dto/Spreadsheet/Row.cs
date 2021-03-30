using System.Collections.Generic;

namespace Spreadsheet.Core.Dto.Spreadsheet
{
    public class Row
    {
        private string _rowKey;
        public int RowIndex { get; set; }

        public string RowKey
        {
            get => _rowKey;
            set => _rowKey = value?.Trim();
        }

        public IList<object> Cells { get; set; }
        
        public Row(int rowIndex, string rowKey, IList<object> cells)
        {
            RowIndex = rowIndex;
            RowKey = rowKey;
            Cells = cells;
        }

        public Row(int columns)
        {
            Cells = new List<object>();

            for (var i = 0; i <= columns; i++)
            {
                Cells.Add(new object());
            }
        }

    }
}