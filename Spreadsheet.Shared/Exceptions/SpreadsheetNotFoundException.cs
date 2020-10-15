using System;

namespace Spreadsheet.Shared.Exceptions
{
    public class SpreadsheetNotFoundException : Exception
    {
        public SpreadsheetNotFoundException(string message) : base(message)
        {
        }
    }
}