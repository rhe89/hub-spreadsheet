using System.Threading.Tasks;
using Spreadsheet.Dto.Spreadsheet;

namespace Spreadsheet.SpreadsheetTabReaders
{
    public interface ITabReader<TTabDto> where TTabDto : TabDto, new()
    {
        Task<TTabDto> GetTab();
    }
}