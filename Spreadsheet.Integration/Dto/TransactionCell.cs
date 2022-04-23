using System.Globalization;
using JetBrains.Annotations;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration.Dto;

[UsedImplicitly]
public class TransactionCell : Hub.Shared.DataContracts.Banking.Dto.TransactionDto, ICell
{
    public string RowKey => Description;
    public string CellValue => Amount.ToString(CultureInfo.CurrentCulture);
}