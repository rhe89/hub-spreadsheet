using System;
using System.Collections.Generic;
using System.Linq;
using SpreadsheetMetadata = Spreadsheet.Data.Documents.SpreadsheetMetadata;

namespace Spreadsheet.Data.Dto;

public class SpreadsheetMetadataDto
{
    public string SpreadsheetId { get; set; }
    public string Name { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public IList<Tab> Tabs { get; set; } = new List<Tab>();

    public SpreadsheetMetadata MapToEntity()
    {
        return new SpreadsheetMetadata
        {
            Id = SpreadsheetId,
            SpreadsheetId = SpreadsheetId,
            Name = Name,
            ValidFrom = ValidFrom,
            ValidTo = ValidTo,
            Tabs = Tabs.Select(tab =>
                new Spreadsheet.Data.Documents.Tab
                {
                    Name = tab.Name,
                    FirstColumn = tab.FirstColumn,
                    LastColumn = tab.LastColumn
                }).ToList()
        };
    }
        
    [Serializable]
    public class Tab
    {
        public string Name { get; set; }

        public string FirstColumn { get; set; }
        
        public string LastColumn { get; set; }
    }
}