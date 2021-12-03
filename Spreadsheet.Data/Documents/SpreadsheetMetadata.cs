using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Spreadsheet.Data.Documents
{
    public class SpreadsheetMetadata
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        public string SpreadsheetId { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        
        public virtual ICollection<Tab> Tabs { get; set; }
    }
    
    public class Tab
    {
        public string Name { get; set; }
        public string FirstColumn { get; set; }
        public string LastColumn { get; set; }
        
        public virtual ICollection<Row> Rows { get; set; }
    }
    
    public class Row
    {
        public string RowKey { get; set; }
        public string Tags { get; set; }
    }
}