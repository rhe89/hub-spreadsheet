using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Spreadsheet;

namespace Spreadsheet.Core.SpreadsheetTabReaders
{
    public interface ITabReader<TTabDto> where TTabDto : TabDtoBase, new()
    {
        Task<TTabDto> GetTab();
    }
}