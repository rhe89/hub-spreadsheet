using System;
using System.Linq;
using System.Threading.Tasks;
using Hub.Storage.Repository;
using Microsoft.Extensions.DependencyInjection;
using Spreadsheet.Data.Entities;
using Spreadsheet.Dto.Spreadsheet;
using Spreadsheet.Shared.Constants;

namespace Spreadsheet.Providers
{
    public class SpreadsheetProvider : ISpreadsheetProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public SpreadsheetProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task<SpreadsheetMetadataDto> CurrentBudgetSpreadsheet()
        {
            using var scope = _serviceProvider.CreateScope();

            using var dbRepository = scope.ServiceProvider.GetService<IScopedDbRepository>();
            
            var spreadsheets =
                await dbRepository.GetManyAsync<SpreadsheetMetadata>(x =>
                        x.Name == SpreadsheetMetadataConstants.BudgetSpreadsheetName &&
                        x.ValidFrom < DateTime.Now && (x.ValidTo == null || x.ValidTo > DateTime.Now),
                    nameof(SpreadsheetMetadata.SpreadsheetTabMetadata),
                    $"{nameof(SpreadsheetMetadata.SpreadsheetTabMetadata)}.{nameof(SpreadsheetTabMetadata.SpreadsheetRowMetadata)}");

            return GetSpreadsheetMetadataDto(spreadsheets.FirstOrDefault());
        }

        private static SpreadsheetMetadataDto GetSpreadsheetMetadataDto(SpreadsheetMetadata spreadsheetMetadata)
        {
            if (spreadsheetMetadata == null)
            {
                return null;
            }
            
            return new SpreadsheetMetadataDto
            {
                Id = spreadsheetMetadata.Id,
                SpreadsheetId = spreadsheetMetadata.SpreadsheetId,
                Name = spreadsheetMetadata.Name,
                ValidFrom = spreadsheetMetadata.ValidFrom,
                ValidTo = spreadsheetMetadata.ValidTo,
                SpreadsheetTabMetadataDtos = spreadsheetMetadata.SpreadsheetTabMetadata.Select(spreadsheetTabMetadata =>
                    new SpreadsheetTabMetadataDto
                    {
                        Name = spreadsheetTabMetadata.Name,
                        FirstColumn = spreadsheetTabMetadata.FirstColumn,
                        LastColumn = spreadsheetTabMetadata.LastColumn,
                        SpreadsheetRowMetadataDtos = spreadsheetTabMetadata.SpreadsheetRowMetadata.Select(
                            spreadsheetRowMetadata => new SpreadsheetRowMetadataDto
                            {
                                RowKey = spreadsheetRowMetadata.RowKey
                            }).ToList()
                    }).ToList()
            };
        }
    }
}