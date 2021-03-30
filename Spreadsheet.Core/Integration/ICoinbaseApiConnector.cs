using System.Collections.Generic;
using System.Threading.Tasks;
using Hub.Web.Http;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;

namespace Spreadsheet.Core.Integration
{
    public interface ICoinbaseApiConnector : IBankApiConnector
    {
        Task<Response<IList<ExchangeRateDto>>> GetExchangeRates();
    }
}