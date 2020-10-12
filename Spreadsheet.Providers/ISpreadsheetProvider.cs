using System.Threading.Tasks;
using Spreadsheet.Dto.Spreadsheet;

namespace Spreadsheet.Providers
{
    public interface ISpreadsheetProvider
    {
        Task<SpreadsheetMetadataDto> CurrentBudgetSpreadsheet();
    }
}