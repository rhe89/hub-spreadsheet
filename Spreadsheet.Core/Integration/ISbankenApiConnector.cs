using System.Collections.Generic;
using System.Threading.Tasks;
using Hub.Web.Http;
using Spreadsheet.Core.Dto.BackgroundTasks;

namespace Spreadsheet.Core.Integration
{
    public interface ISbankenApiConnector
    {
        Task<Response<IList<AccountDto>>> GetAccounts();
    }
}