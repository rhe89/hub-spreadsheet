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

public interface IIncomeTabProvider
{
    Task<IList<IncomeDto>> GetIncomeList(IncomeQuery query);
}

public class IncomeTabProvider : TabProvider<IncomeTab>, IIncomeTabProvider
{
    public IncomeTabProvider(
        ISpreadsheetMetadataProvider spreadsheetMetadataProvider,
        IGoogleSpreadsheetConnector googleSpreadsheetConnector) : base(spreadsheetMetadataProvider, googleSpreadsheetConnector)
    {
    }

    public async Task<IList<IncomeDto>> GetIncomeList(IncomeQuery query)
    {
        var tab = await GetTab(
            SpreadsheetTabMetadataConstants.IncomeSpreadsheetName,
            SpreadsheetTabMetadataConstants.IncomeTabName, 
            query.FromDate);

        var incomeList = new List<IncomeDto>();

        if (tab == null || !tab.Rows.Any())
        {
            return incomeList;
        }
        
        var dateRow = tab.Rows.First();
        var incomeRow = tab.Rows.Last();
        
        for (var colIndex = 1; colIndex < tab.NumberOfCellsInRows; colIndex++)
        {
            var month = dateRow.Cells[colIndex].ParseDateStringInCell();

            var amount = incomeRow.Cells[colIndex].ParseNumberInCell();
            
            incomeList.Add(new IncomeDto
            {
                Month = month,
                Amount = amount
            });
        }
        
        return incomeList;
    }
}