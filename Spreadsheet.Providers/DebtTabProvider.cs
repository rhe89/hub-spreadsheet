using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hub.Shared.DataContracts.Spreadsheet.Dto;
using Hub.Shared.DataContracts.Spreadsheet.Query;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Shared.Constants;
using Spreadsheet.Shared.Extensions;

namespace Spreadsheet.Providers;

public interface IDebtTabProvider
{
    Task<IList<DebtDto>> GetDebtList(DebtQuery query);
}

public class DebtTabProvider : TabProvider<DebtTab>, IDebtTabProvider
{
    public DebtTabProvider(
        ISpreadsheetMetadataProvider spreadsheetMetadataProvider,
        IGoogleSpreadsheetConnector googleSpreadsheetConnector) : base(spreadsheetMetadataProvider, googleSpreadsheetConnector)
    {
    }

    public async Task<IList<DebtDto>> GetDebtList(DebtQuery query)
    {
        var tab = await GetTab(
            SpreadsheetTabMetadataConstants.DebtSpreadsheetName,
            SpreadsheetTabMetadataConstants.DebtTabName, 
            query.FromDate);

        var debtList = new List<DebtDto>();

        if (tab == null || !tab.Rows.Any())
        {
            return debtList;
        }
        
        foreach (var row in tab.Rows)
        {
            var month = row.Cells[0].ParseDateStringInCell();
            
            if (month == DateTime.MinValue)
            {
                continue;
            }
            
            var mortgageAmount = row.Cells[1].ParseNumberInCell();

            var studentLoanAmount = row.Cells[2].ParseNumberInCell();

            debtList.Add(new DebtDto
            {
                Month = month,
                Mortgage = mortgageAmount,
                StudentLoan = studentLoanAmount
            });
        }
        
        return debtList;
    }
}