using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using SpreadsheetMetadata = Spreadsheet.Data.Documents.SpreadsheetMetadata;

namespace Spreadsheet.Data;

public interface ISpreadsheetCosmosDb
{
    Task<IList<SpreadsheetMetadata>> GetSpreadsheetMetadata();
    Task<IList<SpreadsheetMetadata>> GetSpreadsheetMetadata(string whereClause);
    Task<IOrderedQueryable<SpreadsheetMetadata>> GetSpreadsheetMetadataQueryable();
    Task AddOrUpdateSpreadsheetMetadata(SpreadsheetMetadata spreadsheetMetadata);
}

public class SpreadsheetCosmosDb : ISpreadsheetCosmosDb
{
    private readonly string _connectionString;
    private readonly string _databaseId;
    private readonly string _spreadsheetMetadataContainerId;

    public SpreadsheetCosmosDb(IConfiguration configuration)
    {
        _connectionString = configuration.GetValue<string>("COSMOS_DB_CONNECTION_STRING");
        _databaseId = configuration.GetValue<string>("COSMOS_DB_SPREADSHEET");
        _spreadsheetMetadataContainerId = "SpreadsheetMetadata";
    }

    public Task<IList<SpreadsheetMetadata>> GetSpreadsheetMetadata()
    {
        return GetSpreadsheetMetadata(null);
    }

    public async Task<IList<SpreadsheetMetadata>> GetSpreadsheetMetadata(string whereClause)
    {
        var container = await GetContainer();

        var results = new List<SpreadsheetMetadata>();

        const string selectClause = "SELECT * FROM c";

        var queryDefinition = whereClause != null
            ? new QueryDefinition(selectClause + whereClause)
            : new QueryDefinition(selectClause);

        using var feedIterator = container.GetItemQueryIterator<SpreadsheetMetadata>(queryDefinition);

        while (feedIterator.HasMoreResults)
        {
            results.AddRange(await feedIterator.ReadNextAsync());
        }

        return results;
    }

    public async Task<IOrderedQueryable<SpreadsheetMetadata>> GetSpreadsheetMetadataQueryable()
    {
        var container = await GetContainer();

        return container.GetItemLinqQueryable<SpreadsheetMetadata>(true);
    }

    public async Task AddOrUpdateSpreadsheetMetadata(SpreadsheetMetadata spreadsheetMetadata)
    {
        var container = await GetContainer();

        try
        {
            await container.ReadItemAsync<SpreadsheetMetadata>(spreadsheetMetadata.Id,
                new PartitionKey(spreadsheetMetadata.SpreadsheetId));

            await container.ReplaceItemAsync(spreadsheetMetadata, spreadsheetMetadata.Id,
                new PartitionKey(spreadsheetMetadata.SpreadsheetId));
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            await container.CreateItemAsync(spreadsheetMetadata,
                new PartitionKey(spreadsheetMetadata.SpreadsheetId));
        }
    }

    private async Task<Container> GetContainer()
    {
        var cosmosClient = new CosmosClient(_connectionString);

        var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId);

        var container =
            await database.Database.CreateContainerIfNotExistsAsync(_spreadsheetMetadataContainerId,
                "/SpreadsheetId");

        return container;
    }
}