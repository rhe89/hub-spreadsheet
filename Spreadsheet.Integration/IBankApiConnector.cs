using System.Collections.Generic;
using System.Threading.Tasks;
using Hub.Shared.Web.Http;
using Spreadsheet.Integration.Dto;

namespace Spreadsheet.Integration
{
    public interface IBankApiConnector
    {
        string FriendlyApiName { get; }

        Task<Response<IList<AccountDto>>> GetAccounts();
    }
}