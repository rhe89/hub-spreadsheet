using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spreadsheet.Core.Dto.BackgroundTasks;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;

namespace Spreadsheet.Core.SpreadsheetTabWriters
{
    public interface IBankAccountsTabWriter<TBankAccountsTabDto>
        where TBankAccountsTabDto : BankAccountsTabDto, new()
    {
        Task UpdateTab(IList<AccountDto> accounts, DateTime bankAccountTaskLastRun);
    }
}