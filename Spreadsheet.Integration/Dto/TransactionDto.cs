using JetBrains.Annotations;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Shared.Extensions;

namespace Spreadsheet.Integration.Dto;

[UsedImplicitly]
public class TransactionDto : Hub.Shared.DataContracts.Sbanken.TransactionDto, ICell
{
    public string RowKey { get; init; }
        
    public string CellValue => Amount.ToComma();
}