using System.Linq;
using System.Threading.Tasks;
using Hub.Shared.GoogleApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration;

public interface IGoogleSpreadsheetConnector
{
    Task LoadSpreadsheetTab(Tab tab);

    Task UpdateSpreadsheetTab(Tab tab, int firstColumnRow, int lastColumnRow);
}

public class GoogleSpreadsheetConnector : IGoogleSpreadsheetConnector
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GoogleSpreadsheetConnector> _logger;

    public GoogleSpreadsheetConnector(IConfiguration configuration,
        ILogger<GoogleSpreadsheetConnector> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task UpdateSpreadsheetTab(Tab tab, int firstColumnRow, int lastColumnRow)
    {
        var range = $"{tab.Name}!{tab.FirstColumn}{firstColumnRow}:{tab.LastColumn}{lastColumnRow}";

        var values = tab.Rows
            .Select(row => row.Cells)
            .ToList();
        
        await GoogleSheetService.UpdateValuesInTab(_configuration, tab.SpreadsheetId, range, values);
    }

    public async Task LoadSpreadsheetTab(Tab tab)
    {
        _logger.LogInformation("Getting tab {Tab} in sheet with id {Id} from Google API", tab.Name,
            tab.SpreadsheetId);

        var range = $"{tab.Name}!{tab.FirstColumn}:{tab.LastColumn}";

        var valuesInTab = await GoogleSheetService.GetValuesInTab(_configuration, tab.SpreadsheetId, range);

        tab.PopulateRows(valuesInTab);
    }
}