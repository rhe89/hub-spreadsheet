using System;
using System.Linq;
using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Data;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Exceptions;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.Services
{
    public class TabReaderService<TTab> : ITabReaderService<TTab> 
        where TTab : Tab, new()
    {
        private readonly IGoogleSpreadsheetConnector _googleSpreadsheetConnector;
        private readonly ISpreadsheetProvider _spreadsheetProvider;
        private readonly string _tabName;

        public TabReaderService(ISpreadsheetProvider spreadsheetProvider, 
            IGoogleSpreadsheetConnector googleSpreadsheetConnector,
            string tabName)
        {
            _googleSpreadsheetConnector = googleSpreadsheetConnector;
            _tabName = tabName;
            _spreadsheetProvider = spreadsheetProvider;
        }
        
        public async Task<TTab> GetTab()    
        {
            var spreadsheetMetadata = await _spreadsheetProvider.CurrentBudgetSpreadsheetMetadata();

            if (spreadsheetMetadata == null)
            {
                throw new SpreadsheetNotFoundException("No metadata exists for budget spreadsheet for current date");
            }
            
            var tab = SetTabDtoFromSpreadsheetMetadata(spreadsheetMetadata, _tabName);
            
            await PopulateRowsInTab(tab);

            return tab;
        }

        private static TTab SetTabDtoFromSpreadsheetMetadata(SpreadsheetMetadataDto spreadSheet, string tabName)
        {
            var spreadsheetTabMetadata =
                spreadSheet.SpreadsheetTabMetadata.FirstOrDefault(x =>
                    x.Name == tabName);

            if (spreadsheetTabMetadata == null)
            {
                throw new ArgumentException(
                    $"No metadata exists for {tabName} in SpreadsheetMetadata with id {spreadSheet.Id}",
                    nameof(tabName));
            }

            return new TTab
            {
                Name = spreadsheetTabMetadata.Name,
                SpreadsheetId = spreadSheet.SpreadsheetId,
                FirstColumn = spreadsheetTabMetadata.FirstColumn,
                LastColumn = spreadsheetTabMetadata.LastColumn,
                SpreadsheetRowMetadataDtos = spreadsheetTabMetadata.SpreadsheetRowMetadata
            };
        }

        private async Task PopulateRowsInTab(Tab tab)
        {
            await _googleSpreadsheetConnector.LoadSpreadsheetTab(tab);
        }
    }
}