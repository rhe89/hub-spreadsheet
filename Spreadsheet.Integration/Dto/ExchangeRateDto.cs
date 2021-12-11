using JetBrains.Annotations;
using Spreadsheet.Shared.Extensions;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration.Dto;

[UsedImplicitly]
public class ExchangeRateDto : Hub.Shared.DataContracts.Coinbase.ExchangeRateDto, ICell
{
    public string RowKey => Currency;
        
    public string CellValue => NOKRate.ToComma();
}