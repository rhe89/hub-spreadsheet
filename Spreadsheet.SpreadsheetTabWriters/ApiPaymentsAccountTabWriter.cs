using Spreadsheet.Core.Integration;
using Spreadsheet.Core.SpreadsheetTabWriters;

namespace Spreadsheet.SpreadsheetTabWriters
{
    public class ApiPaymentsAccountTabWriter : TabWriterBase, IApiPaymentsAccountTabWriter
    {
        public ApiPaymentsAccountTabWriter(IGoogleSpreadsheetConnector googleSpreadsheetConnector) : base(googleSpreadsheetConnector)
        {
        }
    }
}