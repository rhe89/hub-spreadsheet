using System;
using System.Linq;
using System.Threading.Tasks;
using Spreadsheet.Data.Dto;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Providers;

public abstract class TabProvider<TTab>
    where TTab : Tab, new()
{
    private readonly ISpreadsheetMetadataProvider _spreadsheetMetadataProvider;
    private readonly IGoogleSpreadsheetConnector _googleSpreadsheetConnector;

    protected TabProvider(
        ISpreadsheetMetadataProvider spreadsheetMetadataProvider,
        IGoogleSpreadsheetConnector googleSpreadsheetConnector)
    {
        _spreadsheetMetadataProvider = spreadsheetMetadataProvider;
        _googleSpreadsheetConnector = googleSpreadsheetConnector;
    }
        
    protected static TTab SetTabDtoFromSpreadsheetMetadata(SpreadsheetMetadataDto spreadSheet, string tabName)
    {
        var spreadsheetTabMetadata =
            spreadSheet.Tabs.FirstOrDefault(x =>
                x.Name == tabName);

        if (spreadsheetTabMetadata == null)
        {
            throw new ArgumentException(
                $"No metadata exists for {tabName} in SpreadsheetMetadata with id {spreadSheet.SpreadsheetId}",
                nameof(tabName));
        }

        var tab = new TTab
        {
            Name = spreadsheetTabMetadata.Name,
            SpreadsheetId = spreadSheet.SpreadsheetId,
            FirstColumn = spreadsheetTabMetadata.FirstColumn,
            LastColumn = spreadsheetTabMetadata.LastColumn
        };
        
        tab.Init();
        
        return tab;
    }
    
    protected async Task<TTab> GetTab(string spreadsheetName, string tabName, DateTime fromDate)
    {
        var spreadsheetMetadata = await _spreadsheetMetadataProvider.Get(spreadsheetName, fromDate);

        if (spreadsheetMetadata == null)
        {
            return new TTab();
        }

        var tab = SetTabDtoFromSpreadsheetMetadata(spreadsheetMetadata, tabName);

        await PopulateRowsInTab(tab);

        return tab;
    }

    protected async Task PopulateRowsInTab(Tab tab)
    {
        await _googleSpreadsheetConnector.LoadSpreadsheetTab(tab);
    }
}