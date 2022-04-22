using JetBrains.Annotations;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Shared.Extensions;

namespace Spreadsheet.Integration.Dto;

[UsedImplicitly]
public class TransactionCell : Hub.Shared.DataContracts.Banking.Dto.TransactionDto, ICell
{
    public string RowKey { get; init; }
        
    public string CellValue => Amount.ToComma();
}