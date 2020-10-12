using System.Threading.Tasks;
using Spreadsheet.Dto.Spreadsheet;

namespace Spreadsheet.SpreadsheetTabReaders
{
    public interface IApiDataTabReader
    {
        Task<BudgetSpreadsheetTabDto> GetTab();
    }
}