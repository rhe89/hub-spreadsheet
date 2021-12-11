using System.Collections.Generic;
using System.Threading.Tasks;
using Spreadsheet.Integration.Dto;

namespace Spreadsheet.Integration
{
    public interface IBankApiConnector
    {
        string FriendlyApiName { get; }

        Task<IList<AccountDto>> GetAccounts();
    }
}