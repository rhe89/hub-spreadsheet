using System.Collections.Generic;
using System.Linq;

namespace Spreadsheet.Integration.Dto.Spreadsheet
{
    public class Row
    {
        private string _rowKey;
        private IList<object> _cells;
        public int RowIndex { get; }

        public string RowKey
        {
            get => _rowKey;
            private set => _rowKey = value?.Trim();
        }

        public IList<object> Cells
        {
            get => _cells;
            private set
            {
                _cells = value.Select(x => (object)((string)x).Trim()).ToList();
            }
        }

        public Row(int rowIndex, string rowKey, IList<object> cells)
        {
            RowIndex = rowIndex;
            RowKey = rowKey;
            Cells = cells;
        }

        public Row(int columns)
        {
            _cells = new List<object>();

            for (var i = 0; i <= columns; i++)
            {
                Cells.Add("");
            }
        }

    }
}