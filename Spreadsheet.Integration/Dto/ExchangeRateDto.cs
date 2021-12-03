using Spreadsheet.Shared.Extensions;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration.Dto
{
    public class ExchangeRateDto : Cell
    {
        public string Currency { get; set; }
        public decimal NOKRate { get; set; }
        public decimal USDRate { get; set; }
        public decimal EURRate { get; set; }
        
        public override string RowKey => Currency;
        public override string CellValue => NOKRate.ToComma();
    }
}