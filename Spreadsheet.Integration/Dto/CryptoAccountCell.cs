using JetBrains.Annotations;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Shared.Extensions;

namespace Spreadsheet.Integration.Dto;

[UsedImplicitly]
public class CryptoAccountCell : Hub.Shared.DataContracts.Crypto.Dto.AccountDto, ICell
{
    public string RowKey => Currency;
        
    public string CellValue => Balance.ToComma();
}