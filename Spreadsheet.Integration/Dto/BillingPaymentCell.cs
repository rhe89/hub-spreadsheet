using JetBrains.Annotations;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration.Dto;

[UsedImplicitly]
public class BillingPaymentCell : ICell
{
    public string RowKey { get; init; }
    public string CellValue { get; init; }
}