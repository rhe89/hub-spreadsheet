using System;
using System.Linq;
using System.Threading.Tasks;
using Spreadsheet.Data.Dto;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;
using Spreadsheet.Providers;

namespace Spreadsheet.Services
{
    public interface ITabReaderService<TTabDto> where TTabDto : Tab, new()
    {
        Task<TTabDto> GetTab();
    }
    
    public class TabReaderService<TTab> : ITabReaderService<TTab> 
        where TTab : Tab, new()
    {
        private readonly IGoogleSpreadsheetConnector _googleSpreadsheetConnector;
        private readonly ISpreadsheetMetadataProvider _spreadsheetMetadataProvider;
        private readonly string _tabName;

        public TabReaderService(ISpreadsheetMetadataProvider spreadsheetMetadataProvider, 
            IGoogleSpreadsheetConnector googleSpreadsheetConnector,
            string tabName)
        {
            _googleSpreadsheetConnector = googleSpreadsheetConnector;
            _tabName = tabName;
            _spreadsheetMetadataProvider = spreadsheetMetadataProvider;
        }
        
        public async Task<TTab> GetTab()    
        {
            var spreadsheetMetadata = await _spreadsheetMetadataProvider.GetCurrentBudgetSpreadsheetMetadata();

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
                spreadSheet.Tabs.FirstOrDefault(x =>
                    x.Name == tabName);

            if (spreadsheetTabMetadata == null)
            {
                throw new ArgumentException(
                    $"No metadata exists for {tabName} in SpreadsheetMetadata with id {spreadSheet.SpreadsheetId}",
                    nameof(tabName));
            }

            return new TTab
            {
                Name = spreadsheetTabMetadata.Name,
                SpreadsheetId = spreadSheet.SpreadsheetId,
                FirstColumn = spreadsheetTabMetadata.FirstColumn,
                LastColumn = spreadsheetTabMetadata.LastColumn,
                SpreadsheetRowMetadataDtos = spreadsheetTabMetadata.Rows
            };
        }

        private async Task PopulateRowsInTab(Tab tab)
        {
            await _googleSpreadsheetConnector.LoadSpreadsheetTab(tab);
        }
        
        private class SpreadsheetNotFoundException : Exception
        {
            public SpreadsheetNotFoundException(string message) : base(message)
            {
            }
        }
    }
}