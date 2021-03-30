using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Spreadsheet;

namespace Spreadsheet.Core.Integration
{
    public interface IGoogleSpreadsheetConnector
    {
        Task LoadSpreadsheetTab(Tab tab);

        Task UpdateSpreadsheetTab(Tab tab, int firstColumnRow, int lastColumnRow);
    }
}