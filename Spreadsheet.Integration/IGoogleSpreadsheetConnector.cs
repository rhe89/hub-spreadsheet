using System.Threading.Tasks;
using Spreadsheet.Dto.Spreadsheet;

namespace Spreadsheet.Integration
{
    public interface IGoogleSpreadsheetConnector
    {
        Task LoadSpreadsheetTab(TabDto tabDto);

        Task UpdateSpreadsheetTab(TabDto tabDto, int firstColumnRow, int lastColumnRow);
    }
}