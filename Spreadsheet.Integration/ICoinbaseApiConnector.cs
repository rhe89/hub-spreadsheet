using System.Collections.Generic;
using System.Threading.Tasks;
using Hub.Web.Http;
using Spreadsheet.Dto.Account;

namespace Spreadsheet.Integration
{
    public interface ICoinbaseApiConnector
    {
        Task<Response<IList<AccountDto>>> GetAccounts();
    }
}