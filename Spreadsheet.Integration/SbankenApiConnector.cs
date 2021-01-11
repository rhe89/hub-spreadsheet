using System.Net.Http;
using Spreadsheet.Core.Integration;

namespace Spreadsheet.Integration
{
    public class SbankenApiConnector : BankApiConnector, ISbankenApiConnector
    {
        public SbankenApiConnector(HttpClient httpClient) : base(httpClient, "SbankenApi") {}
    }
}