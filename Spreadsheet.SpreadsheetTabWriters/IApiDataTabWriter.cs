using System.Collections.Generic;
using System.Threading.Tasks;
using Spreadsheet.Dto.Account;

namespace Spreadsheet.SpreadsheetTabWriters
{
    public interface IApiDataTabWriter
    {
        Task UpdateTab(IList<AccountDto> accounts);
        Task UpdateTab(AccountDto account);
    }
}