using JetBrains.Annotations;
using Spreadsheet.Shared.Extensions;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration.Dto;

[UsedImplicitly]
public class ExchangeRateCell : Hub.Shared.DataContracts.Crypto.Dto.ExchangeRateDto, ICell
{
    public string RowKey => Currency;
        
    public string CellValue => NOKRate.ToComma();
}