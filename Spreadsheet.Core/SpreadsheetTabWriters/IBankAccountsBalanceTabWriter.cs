using System.Collections.Generic;
using System.Threading.Tasks;
using Spreadsheet.Core.Dto.BackgroundTasks;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;

namespace Spreadsheet.Core.SpreadsheetTabWriters
{
    public interface IBankAccountsBalanceTabWriter<TBankAccountsTabDto>
        where TBankAccountsTabDto : BankAccountsTabDto, new()
    {
        Task UpdateTab(IList<AccountDto> accounts);
    }
}