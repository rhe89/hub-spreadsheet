using System.Collections.Generic;
using System.Threading.Tasks;
using Hub.Storage.Core.Dto;
using Hub.Web.Http;
using Spreadsheet.Core.Dto.Integration;

namespace Spreadsheet.Core.Integration
{
    public interface IBankApiConnector
    {
        string FriendlyApiName { get; }

        Task<Response<IList<AccountDto>>> GetAccounts();
        Task<Response<IList<BackgroundTaskConfigurationDto>>> GetBackgroundTaskConfigurations();
    }
}