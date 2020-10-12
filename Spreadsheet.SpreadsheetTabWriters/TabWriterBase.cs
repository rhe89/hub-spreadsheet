using System.Linq;
using System.Threading.Tasks;
using Spreadsheet.Dto.Spreadsheet;
using Spreadsheet.Integration;

namespace Spreadsheet.SpreadsheetTabWriters
{
    public abstract class TabWriterBase
    {
        private readonly IGoogleSpreadsheetConnector _googleSpreadsheetConnector;

        protected TabWriterBase(IGoogleSpreadsheetConnector googleSpreadsheetConnector)
        {
            _googleSpreadsheetConnector = googleSpreadsheetConnector;
        }

        protected async Task UpdateSheet(TabDto tabDto, int firstColumnRow, int lastColumnRow)
        {
            await _googleSpreadsheetConnector.UpdateSpreadsheetTab(tabDto, firstColumnRow, lastColumnRow);
        }
    }
}