using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Integration;

namespace Spreadsheet.SpreadsheetTabWriters
{
    public abstract class TabWriterBase
    {
        private readonly IGoogleSpreadsheetConnector _googleSpreadsheetConnector;

        protected TabWriterBase(IGoogleSpreadsheetConnector googleSpreadsheetConnector)
        {
            _googleSpreadsheetConnector = googleSpreadsheetConnector;
        }

        protected async Task UpdateSheet(TabDtoBase tabDtoBase, int firstColumnRow, int lastColumnRow)
        {
            await _googleSpreadsheetConnector.UpdateSpreadsheetTab(tabDtoBase, firstColumnRow, lastColumnRow);
        }
    }
}