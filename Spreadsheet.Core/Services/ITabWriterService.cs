using System.Collections.Generic;
using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Spreadsheet;

namespace Spreadsheet.Core.Services
{
    public interface ITabWriterService<TTab>
        where TTab : Tab, new()
    {
        Task UpdateTab(IList<Cell> rows);
    }
}