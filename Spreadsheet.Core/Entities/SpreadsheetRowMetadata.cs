using Hub.Storage.Repository.Entities;

namespace Spreadsheet.Core.Entities
{
    public class SpreadsheetRowMetadata : EntityBase
    {
        public long SpreadsheetTabMetadataId { get; set; }
        public string RowKey { get; set; }
        public string Tags { get; set; }

        public virtual SpreadsheetTabMetadata SpreadsheetTabMetadata { get; set; }
        
    }
}