using System.Globalization;
using JetBrains.Annotations;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration.Dto;

[UsedImplicitly]
public class CryptoAccountCell : Hub.Shared.DataContracts.Crypto.Dto.AccountDto, ICell
{
    public string RowKey => Currency;
    public string CellValue => Balance.ToString(CultureInfo.CurrentCulture);
}