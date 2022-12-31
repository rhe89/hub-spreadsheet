using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using SpreadsheetMetadata = Spreadsheet.Data.Documents.SpreadsheetMetadata;

namespace Spreadsheet.Data;

public interface ICosmosDbContext
{
    Task<IOrderedQueryable<SpreadsheetMetadata>> GetSpreadsheetMetadataQueryable();
}

public class CosmosDbContextContext : ICosmosDbContext
{
    private readonly string _connectionString;
    private readonly string _databaseId;
    private readonly string _spreadsheetMetadataContainerId;

    public CosmosDbContextContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetValue<string>("COSMOS_DB_CONNECTION_STRING");
        _databaseId = configuration.GetValue<string>("COSMOS_DB_SPREADSHEET");
        _spreadsheetMetadataContainerId = "SpreadsheetMetadata";
    }

    public async Task<IOrderedQueryable<SpreadsheetMetadata>> GetSpreadsheetMetadataQueryable()
    {
        var container = await GetContainer();

        return container.GetItemLinqQueryable<SpreadsheetMetadata>(true);
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