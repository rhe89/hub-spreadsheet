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
    Task<SpreadsheetMetadataDto> GetSpreadsheet(string id);
    Task<IList<SpreadsheetMetadataDto>> GetSpreadsheets();
    Task<SpreadsheetMetadataDto> GetCurrentBudgetSpreadsheetMetadata();
    Task<IList<SpreadsheetMetadataDto.Row>> GetRowsInTabForCurrentSpreadsheet(string tabName);
}

public class SpreadsheetMetadataProvider : ISpreadsheetMetadataProvider
{
    private readonly ISpreadsheetCosmosDb _spreadsheetCosmosDb;

    public SpreadsheetMetadataProvider(ISpreadsheetCosmosDb spreadsheetCosmosDb)
    {
        _spreadsheetCosmosDb = spreadsheetCosmosDb;
    }

    public async Task<SpreadsheetMetadataDto> GetSpreadsheet(string id)
    {
        var queryable = await _spreadsheetCosmosDb
            .GetSpreadsheetMetadataQueryable();

        var spreadsheets = queryable.Where(x => x.Id == id).ToList();

        return Map(spreadsheets.FirstOrDefault());
    }

    public async Task<SpreadsheetMetadataDto> GetCurrentBudgetSpreadsheetMetadata()
    {
        var queryable = await _spreadsheetCosmosDb
            .GetSpreadsheetMetadataQueryable();

        var spreadsheets = queryable.Where(x =>
                DateTime.Now >= x.ValidFrom &&
                DateTime.Now < x.ValidTo)
            .ToList();

        return Map(spreadsheets.FirstOrDefault());
    }

    public async Task<IList<SpreadsheetMetadataDto.Row>> GetRowsInTabForCurrentSpreadsheet(string tabName)
    {
        var spreadsheet = await GetCurrentBudgetSpreadsheetMetadata();

        return spreadsheet?.Tabs?.FirstOrDefault(x => x.Name == tabName)?.Rows;
    }

    public async Task<IList<SpreadsheetMetadataDto>> GetSpreadsheets()
    {
        var spreadsheets = await _spreadsheetCosmosDb.GetSpreadsheetMetadata();

        return spreadsheets.Select(Map).ToList();
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
                    LastColumn = tab.LastColumn,
                    Rows = tab.Rows?.Select(row => new SpreadsheetMetadataDto.Row
                    {
                        RowKey = row.RowKey,
                        Tags = row.Tags
                    }).ToList()
                }).ToList()
        };
    }
}