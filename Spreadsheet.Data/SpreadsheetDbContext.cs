using Hub.Shared.Storage.Repository;
using Microsoft.EntityFrameworkCore;

namespace Spreadsheet.Data;

public class SpreadsheetDbContext : HubDbContext
{
    public SpreadsheetDbContext(DbContextOptions<SpreadsheetDbContext> options) : base(options) { }
}