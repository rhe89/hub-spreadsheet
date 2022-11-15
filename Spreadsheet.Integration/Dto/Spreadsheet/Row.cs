using System.Collections.Generic;
using System.Linq;

namespace Spreadsheet.Integration.Dto.Spreadsheet;

public class Row
{
    private IList<object> _cells;

    public string RowKey
    {
        get => Cells.First().ToString();
        set => Cells[0] = value?.Trim();
    }

    public IList<object> Cells
    {
        get => _cells;
        set => _cells = value.Select(x => (object)((string)x).Trim()).ToList();
    }

    public Row(IList<object> cells)
    {
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