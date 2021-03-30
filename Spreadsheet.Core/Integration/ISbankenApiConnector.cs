using System.Collections.Generic;
using System.Threading.Tasks;
using Hub.Web.Http;
using Spreadsheet.Core.Dto.Integration;

namespace Spreadsheet.Core.Integration
{
    public interface ISbankenApiConnector : IBankApiConnector
    {
        Task<Response<IList<TransactionDto>>> GetBillingAccountTransactions();
    }
}