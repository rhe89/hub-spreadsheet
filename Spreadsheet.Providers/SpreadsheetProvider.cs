using System;
using System.Threading.Tasks;
using Hub.Storage.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Data;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Entities;
using Spreadsheet.Core.Providers;

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
            
            using var dbRepository = scope.ServiceProvider.GetService<IScopedHubDbRepository>();

            var spreadsheet = await dbRepository
                .Where<SpreadsheetMetadata>(x => x.Name == SpreadsheetMetadataConstants.BudgetSpreadsheetName &&
                                                 x.ValidFrom < DateTime.Now && (x.ValidTo == null || x.ValidTo > DateTime.Now))
                .Include(x => x.SpreadsheetTabMetadata)
                    .ThenInclude(x => x.SpreadsheetRowMetadata)
                .FirstOrDefaultAsync();

            return dbRepository.Map<SpreadsheetMetadata, SpreadsheetMetadataDto>(spreadsheet);
        }
    }
}