using System;
using System.Threading.Tasks;
using Hub.Storage.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Data;
using Spreadsheet.Core.Entities;
using Spreadsheet.Core.Providers;

namespace Spreadsheet.Providers
{
    public class SpreadsheetProvider : ISpreadsheetProvider
    {
        private readonly IHubDbRepository _dbRepository;

        public SpreadsheetProvider(IHubDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        
        public async Task<SpreadsheetMetadataDto> CurrentBudgetSpreadsheet()
        {
            var spreadsheet = await _dbRepository
                .Where<SpreadsheetMetadata>(x => x.Name == SpreadsheetMetadataConstants.BudgetSpreadsheetName &&
                                                 x.ValidFrom < DateTime.Now && (x.ValidTo == null || x.ValidTo > DateTime.Now))
                .Include(x => x.SpreadsheetTabMetadata)
                    .ThenInclude(x => x.SpreadsheetRowMetadata)
                .FirstOrDefaultAsync();

            return _dbRepository.Map<SpreadsheetMetadata, SpreadsheetMetadataDto>(spreadsheet);
        }
    }
}