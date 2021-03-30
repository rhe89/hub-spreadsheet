using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Spreadsheet;

namespace Spreadsheet.Core.Services
{
    public interface ITabReaderService<TTabDto> where TTabDto : Tab, new()
    {
        Task<TTabDto> GetTab();
    }
}