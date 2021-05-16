using System;

namespace Spreadsheet.Core.Exceptions
{
    public class ApiConnectorException : Exception
    {
        public ApiConnectorException(string responseErrorMessage) : base(responseErrorMessage)
        {
            
        }
    }
}