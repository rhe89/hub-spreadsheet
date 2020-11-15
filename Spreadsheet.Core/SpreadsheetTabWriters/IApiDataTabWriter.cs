using System.Collections.Generic;
using System.Threading.Tasks;
using Spreadsheet.Core.Dto.BackgroundTasks;

namespace Spreadsheet.Core.SpreadsheetTabWriters
{
    public interface IApiDataTabWriter
    {
        Task UpdateTab(IList<AccountDto> accounts);
        Task UpdateTab(AccountDto account);
    }
}