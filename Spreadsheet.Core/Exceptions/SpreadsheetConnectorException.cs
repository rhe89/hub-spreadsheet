using System;

namespace Spreadsheet.Core.Exceptions
{
    public class SpreadsheetConnectorException : Exception
    {
        public SpreadsheetConnectorException(string message) : base(message)
        {
        }
        public SpreadsheetConnectorException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}