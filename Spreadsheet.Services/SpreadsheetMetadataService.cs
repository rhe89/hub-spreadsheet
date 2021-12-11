using System.Threading.Tasks;
using Spreadsheet.Data;
using Spreadsheet.Data.Dto;
using Spreadsheet.Providers;

namespace Spreadsheet.Services
{
    public interface ISpreadsheetMetadataService
    {
        Task CreateOrUpdateSpreadsheetMetadata(SpreadsheetMetadataDto spreadsheetMetadataDto);
        Task<SpreadsheetMetadataDto> InitializeSpreadsheetMetadataCopy(string id);
        SpreadsheetMetadataDto InitializeEmptySpreadsheetMetadata();
    }

    public class SpreadsheetMetadataService : ISpreadsheetMetadataService
    {
        private readonly ISpreadsheetCosmosDb _spreadsheetCosmosDb;
        private readonly ISpreadsheetMetadataProvider _spreadsheetMetadataProvider;

        public SpreadsheetMetadataService(ISpreadsheetMetadataProvider spreadsheetMetadataProvider,
            ISpreadsheetCosmosDb spreadsheetCosmosDb)
        {
            _spreadsheetMetadataProvider = spreadsheetMetadataProvider;
            _spreadsheetCosmosDb = spreadsheetCosmosDb;
        }

        public async Task CreateOrUpdateSpreadsheetMetadata(SpreadsheetMetadataDto spreadsheetMetadataDto)
        {
            await _spreadsheetCosmosDb.AddOrUpdateSpreadsheetMetadata(spreadsheetMetadataDto.MapToEntity());
        }

        public async Task<SpreadsheetMetadataDto> InitializeSpreadsheetMetadataCopy(string id)
        {
            var spreadsheet = await _spreadsheetMetadataProvider.GetSpreadsheet(id);

            return spreadsheet;
        }

        public SpreadsheetMetadataDto InitializeEmptySpreadsheetMetadata()
        {
            var spreadsheet = new SpreadsheetMetadataDto();

            var spreadsheetTabMetadata = new SpreadsheetMetadataDto.Tab();

            spreadsheetTabMetadata.Rows.Add(new SpreadsheetMetadataDto.Row());

            spreadsheet.Tabs.Add(spreadsheetTabMetadata);

            return spreadsheet;
        }
    }
}