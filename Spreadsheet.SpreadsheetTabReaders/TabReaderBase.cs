using System;
using System.Linq;
using System.Threading.Tasks;
using Spreadsheet.Dto.Spreadsheet;
using Spreadsheet.Integration;
using Spreadsheet.Providers;

namespace Spreadsheet.SpreadsheetTabReaders
{
    public abstract class TabReaderBase
    {
        private readonly IGoogleSpreadsheetConnector _googleSpreadsheetConnector;
        private readonly ISpreadsheetProvider _spreadsheetProvider;

        protected TabReaderBase(ISpreadsheetProvider spreadsheetProvider, 
            IGoogleSpreadsheetConnector googleSpreadsheetConnector)
        {
            _googleSpreadsheetConnector = googleSpreadsheetConnector;
            _spreadsheetProvider = spreadsheetProvider;
        }

        protected async Task<SpreadsheetMetadataDto> GetCurrentBudgetSpreadsheetMetadataDto()
        {
            var spreadsheetMetadataDto = await _spreadsheetProvider.CurrentBudgetSpreadsheet();
            
            if (spreadsheetMetadataDto == null) 
                throw new Exception("No metadata exists for budget spreadsheet for current date");

            return spreadsheetMetadataDto;
        }
        
        protected static TTabDto SetTabDtoFromSpreadsheetMetadata<TTabDto>(SpreadsheetMetadataDto spreadSheet, string tabName) where TTabDto : TabDto, new()
        {
            var spreadsheetTabMetadata =
                spreadSheet.SpreadsheetTabMetadataDtos.FirstOrDefault(x =>
                    x.Name == tabName);

            if (spreadsheetTabMetadata == null)
                throw new Exception(
                    $"No metadata exists for {tabName} in SpreadsheetMetadata with id {spreadSheet.Id}");

            return new TTabDto()
            {
                Name = spreadsheetTabMetadata.Name,
                SpreadsheetId = spreadSheet.SpreadsheetId,
                FirstColumn = spreadsheetTabMetadata.FirstColumn,
                LastColumn = spreadsheetTabMetadata.LastColumn,
                SpreadsheetRowMetadataDtos = spreadsheetTabMetadata.SpreadsheetRowMetadataDtos
            };
        }
        
        protected async Task PopulateRowsInTabDto(TabDto tabDto)
        {
            await _googleSpreadsheetConnector.LoadSpreadsheetTab(tabDto);
        }
    }
}