using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Spreadsheet.Data.Documents
{
    [JsonObject]
    public class SpreadsheetMetadata
    {
        [JsonProperty(PropertyName = "id")] 
        public string Id { get; set; }

        [JsonProperty] 
        public string SpreadsheetId { get; set; }

        [JsonProperty] 
        public string Name { get; set; }

        [JsonProperty] 
        public DateTime ValidFrom { get; set; }

        [JsonProperty] 
        public DateTime ValidTo { get; set; }

        [JsonProperty] 
        public virtual ICollection<Tab> Tabs { get; set; }
    }

    [JsonObject]
    public class Tab
    {
        [JsonProperty] 
        public string Name { get; set; }
        
        [JsonProperty] 
        public string FirstColumn { get; set; }
        
        [JsonProperty] 
        public string LastColumn { get; set; }

        [JsonProperty] 
        public virtual ICollection<Row> Rows { get; set; }
    }

    [JsonObject]
    public class Row
    {
        [JsonProperty] 
        public string RowKey { get; set; }
        [JsonProperty] 
        public string Tags { get; set; }
    }
}