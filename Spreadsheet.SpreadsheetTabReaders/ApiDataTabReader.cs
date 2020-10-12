using System.Threading.Tasks;
using Spreadsheet.Dto.Spreadsheet;
using Spreadsheet.Integration;
using Spreadsheet.Providers;
using Spreadsheet.Shared.Constants;

namespace Spreadsheet.SpreadsheetTabReaders
{
    public class ApiDataTabReader : TabReaderBase, IApiDataTabReader
    {
        public ApiDataTabReader(ISpreadsheetProvider spreadsheetProvider, 
            IGoogleSpreadsheetConnector googleSpreadsheetConnector) : base(spreadsheetProvider, googleSpreadsheetConnector) { }

        public async Task<BudgetSpreadsheetTabDto> GetTab()
        {
            var apiDataTabDto = await SetTabDtoFromCurrentBudgetSpreadsheet();
            
            await PopulateRowsInTabDto(apiDataTabDto);

            return apiDataTabDto;
        }

        private async Task<BudgetSpreadsheetTabDto> SetTabDtoFromCurrentBudgetSpreadsheet()
        {
            var spreadsheetMetadataDto = await GetCurrentBudgetSpreadsheetMetadataDto();
            
            return SetTabDtoFromSpreadsheetMetadata<BudgetSpreadsheetTabDto>(spreadsheetMetadataDto, SpreadsheetTabMetadataConstants.ApiDataTabName);
        }

        
    }
}