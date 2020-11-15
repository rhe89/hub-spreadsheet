using System.Collections.Generic;
using Hub.Storage.Core.Entities;

namespace Spreadsheet.Core.Entities
{
    public class SpreadsheetTabMetadata : EntityBase
    {
        public long SpreadsheetMetadataId { get; set; }
        public string Name { get; set; }
        public string FirstColumn { get; set; }
        public string LastColumn { get; set; }

        public virtual SpreadsheetMetadata SpreadsheetMetadata { get; set; }
        
        public virtual ICollection<SpreadsheetRowMetadata> SpreadsheetRowMetadata { get; set; }
    }
}