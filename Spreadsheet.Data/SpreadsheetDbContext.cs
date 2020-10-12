using Hub.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Spreadsheet.Data.Entities;

namespace Spreadsheet.Data
{
    public class SpreadsheetDbContext : DbContext
    {
        public SpreadsheetDbContext(DbContextOptions<SpreadsheetDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.AddSettingEntity();
            builder.AddWorkerLogEntity();
            builder.AddBackgroundTaskConfigurationEntity();
            
            builder.Entity<SpreadsheetMetadata>()
                .ToTable(schema: "dbo", name: "SpreadsheetMetadata");
            
            builder.Entity<SpreadsheetTabMetadata>()
                .ToTable(schema: "dbo", name: "SpreadsheetTabMetadata");
            
            builder.Entity<SpreadsheetRowMetadata>()
                .ToTable(schema: "dbo", name: "SpreadsheetRowMetadata");
            
            builder.Entity<AccountTransferPeriod>()
                .ToTable(schema: "dbo", name: "AccountTransferPeriod");
            
            builder.Entity<AccountTransfer>()
                .ToTable(schema: "dbo", name: "AccountTransfer");
        }
    }
}