﻿using System.Globalization;
using JetBrains.Annotations;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration.Dto;

[UsedImplicitly]
public class BankingAccountCell : Hub.Shared.DataContracts.Banking.Dto.AccountDto, ICell
{
    public string RowKey => Name;
    public string CellValue => Balance.ToString(CultureInfo.CurrentCulture);
}