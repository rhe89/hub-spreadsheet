using Spreadsheet.Shared.Extensions;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration.Dto
{
    public class AccountDto : Cell
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public override string RowKey => Name;
        public override string CellValue => Balance.ToComma();
    }
}
