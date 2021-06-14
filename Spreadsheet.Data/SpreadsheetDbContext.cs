using Hub.Storage.Repository;
using Microsoft.EntityFrameworkCore;
using Spreadsheet.Core.Entities;

namespace Spreadsheet.Data
{
    public class SpreadsheetDbContext : HubDbContext
    {
        public SpreadsheetDbContext(DbContextOptions<SpreadsheetDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<SpreadsheetMetadata>()
                .ToTable(schema: "dbo", name: "SpreadsheetMetadata");
            
            builder.Entity<SpreadsheetTabMetadata>()
                .ToTable(schema: "dbo", name: "SpreadsheetTabMetadata");
            
            builder.Entity<SpreadsheetRowMetadata>()
                .ToTable(schema: "dbo", name: "SpreadsheetRowMetadata");
            
            builder.Entity<BillingAccountPayment>()
                .ToTable(schema: "dbo", name: "BillingAccountPayment");
        }
    }
}