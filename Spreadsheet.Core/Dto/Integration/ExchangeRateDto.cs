using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Extensions;

namespace Spreadsheet.Core.Dto.Integration
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