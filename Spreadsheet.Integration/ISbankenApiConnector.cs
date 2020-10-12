using System.Collections.Generic;
using System.Threading.Tasks;
using Hub.Web.Http;
using Spreadsheet.Dto.Account;

namespace Spreadsheet.Integration
{
    public interface ISbankenApiConnector
    {
        Task<Response<IList<AccountDto>>> GetAccounts();
    }
}