using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hub.Storage.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
        
        public async Task<SpreadsheetMetadataDto> CurrentBudgetSpreadsheetMetadata()
        {
            static Func<IQueryable<SpreadsheetMetadata>, IIncludableQueryable<SpreadsheetMetadata, object>> Include() =>
                source => source
                    .Include(x => x.SpreadsheetTabMetadata)
                    .ThenInclude(x => x.SpreadsheetRowMetadata);

            static Expression<Func<SpreadsheetMetadata, bool>> Predicate() => 
                x => x.Name == SpreadsheetMetadataConstants.BudgetSpreadsheetName &&
                                       x.ValidFrom < DateTime.Now && (x.ValidTo == null || x.ValidTo > DateTime.Now);

            
            var spreadsheet = await _dbRepository
                .FirstOrDefaultAsync<SpreadsheetMetadata, SpreadsheetMetadataDto>(Predicate(), Include());

            return spreadsheet;
        }

        public async Task<IList<SpreadsheetRowMetadataDto>> GetRowsInTabForCurrentSpreadsheet(string tabName)
        {
            static Func<IQueryable<SpreadsheetMetadata>, IIncludableQueryable<SpreadsheetMetadata, object>> Include() =>
                source => source
                    .Include(x => x.SpreadsheetTabMetadata)
                    .ThenInclude(x => x.SpreadsheetRowMetadata);

            static Expression<Func<SpreadsheetMetadata, bool>> Predicate() => 
                x => x.Name == SpreadsheetMetadataConstants.BudgetSpreadsheetName &&
                                       x.ValidFrom < DateTime.Now &&
                                      (x.ValidTo == null || x.ValidTo > DateTime.Now);
            
            var spreadsheet = await _dbRepository
                .FirstOrDefaultAsync<SpreadsheetMetadata, SpreadsheetMetadataDto>(Predicate(), Include());

            var rows = spreadsheet
                .SpreadsheetTabMetadata.FirstOrDefault(x => x.Name == tabName)
                ?.SpreadsheetRowMetadata;
            
            return rows;
        }

        
    }
}