using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spreadsheet.Data;
using Spreadsheet.Data.Documents;
using Spreadsheet.Data.Dto;

namespace Spreadsheet.Providers;

public interface ISpreadsheetMetadataProvider
{
    Task<SpreadsheetMetadataDto> Get(string spreadsheetName, DateTime from);
}

public class SpreadsheetMetadataProvider : ISpreadsheetMetadataProvider
{
    private readonly ICosmosDbContext _cosmosDbContext;

    public SpreadsheetMetadataProvider(
        ICosmosDbContext cosmosDbContext)
    {
        _cosmosDbContext = cosmosDbContext;
    }
    
    public async Task<SpreadsheetMetadataDto> Get(string spreadsheetName, DateTime from)
    {
        var queryable = await _cosmosDbContext
            .GetSpreadsheetMetadataQueryable();

        var spreadsheets = queryable
            .Where(
                x => x.Name == spreadsheetName || x.Name.Contains(spreadsheetName) &&
                     from >= x.ValidFrom &&
                     (x.ValidTo == null || from < x.ValidTo))
            .ToList();

        return Map(spreadsheets.FirstOrDefault());
    }

    private static SpreadsheetMetadataDto Map(SpreadsheetMetadata spreadsheetMetadata)
    {
        if (spreadsheetMetadata == null)
        {
            return null;
        }

        return new SpreadsheetMetadataDto
        {
            SpreadsheetId = spreadsheetMetadata.SpreadsheetId,
            Name = spreadsheetMetadata.Name,
            ValidFrom = spreadsheetMetadata.ValidFrom,
            ValidTo = spreadsheetMetadata.ValidTo,
            Tabs = spreadsheetMetadata.Tabs?.Select(tab =>
                new SpreadsheetMetadataDto.Tab
                {
                    Name = tab.Name,
                    FirstColumn = tab.FirstColumn,
                    LastColumn = tab.LastColumn
                }).ToList()
        };
    }
}