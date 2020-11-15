using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Spreadsheet;

namespace Spreadsheet.Core.Integration
{
    public interface IGoogleSpreadsheetConnector
    {
        Task LoadSpreadsheetTab(TabDtoBase tabDtoBase);

        Task UpdateSpreadsheetTab(TabDtoBase tabDtoBase, int firstColumnRow, int lastColumnRow);
    }
}