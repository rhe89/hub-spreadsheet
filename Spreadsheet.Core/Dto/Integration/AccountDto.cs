using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Extensions;

namespace Spreadsheet.Core.Dto.Integration
{
    public class AccountDto : Cell
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public override string RowKey => Name;
        public override string CellValue => Balance.ToComma();
    }
}
