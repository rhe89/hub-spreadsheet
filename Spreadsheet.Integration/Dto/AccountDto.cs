using JetBrains.Annotations;
using Spreadsheet.Shared.Extensions;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration.Dto
{
    [UsedImplicitly]
    public class AccountDto : Hub.Shared.DataContracts.Banking.AccountDto, ICell
    {
        public string RowKey => Name;
        
        public string CellValue => Balance.ToComma();
    }
}