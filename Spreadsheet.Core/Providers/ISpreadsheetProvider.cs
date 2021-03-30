using System.Collections.Generic;
using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Data;

namespace Spreadsheet.Core.Providers
{
    public interface ISpreadsheetProvider
    {
        Task<SpreadsheetMetadataDto> CurrentBudgetSpreadsheetMetadata();
        Task<IList<SpreadsheetRowMetadataDto>> GetRowsInTabForCurrentSpreadsheet(string tabName);
    }
}