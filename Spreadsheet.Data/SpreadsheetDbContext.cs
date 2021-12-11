using Hub.Shared.Storage.Repository;
using Microsoft.EntityFrameworkCore;
using Spreadsheet.Data.Entities;

namespace Spreadsheet.Data;

public class SpreadsheetDbContext : HubDbContext
{
    public SpreadsheetDbContext(DbContextOptions<SpreadsheetDbContext> options) : base(options) { }
        
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
            
        builder.Entity<BillingAccountTransaction>()
            .ToTable(schema: "dbo", name: "BillingAccountPayment");
    }
}