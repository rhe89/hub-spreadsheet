using System.Globalization;
using Hub.Shared.DataContracts.Banking.Dto;
using JetBrains.Annotations;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration.Dto;

[UsedImplicitly]
public class BankingAccountBalanceCell : AccountDto, ICell
{
    public string RowKey => $"{Name} {(Bank != null ? $"({Bank.Name})" : "")}";
    public string CellValue => Balance.ToString(CultureInfo.CurrentCulture);
}