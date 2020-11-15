using Hub.Storage.Core.Entities;

namespace Spreadsheet.Core.Entities
{
    public class SpreadsheetRowMetadata : EntityBase
    {
        public long SpreadsheetTabMetadataId { get; set; }
        public string RowKey { get; set; }

        public virtual SpreadsheetTabMetadata SpreadsheetTabMetadata { get; set; }
        
    }
}