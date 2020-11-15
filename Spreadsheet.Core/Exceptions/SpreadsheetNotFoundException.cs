using System;

namespace Spreadsheet.Core.Exceptions
{
    public class SpreadsheetNotFoundException : Exception
    {
        public SpreadsheetNotFoundException(string message) : base(message)
        {
        }
    }
}