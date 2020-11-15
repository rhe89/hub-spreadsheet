using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Data;
using Spreadsheet.Core.Dto.Spreadsheet;

namespace Spreadsheet.Core.Providers
{
    public interface ISpreadsheetProvider
    {
        Task<SpreadsheetMetadataDto> CurrentBudgetSpreadsheet();
    }
}