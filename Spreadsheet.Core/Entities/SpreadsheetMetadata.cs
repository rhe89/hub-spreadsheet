using System;
using System.Collections.Generic;
using Hub.Storage.Repository.Entities;

namespace Spreadsheet.Core.Entities
{
    public class SpreadsheetMetadata : EntityBase
    {
        public string SpreadsheetId { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual ICollection<SpreadsheetTabMetadata> SpreadsheetTabMetadata { get; set; }
    }
}