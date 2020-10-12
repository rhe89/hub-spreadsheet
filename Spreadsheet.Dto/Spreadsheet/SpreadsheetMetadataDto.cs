using System;
using System.Collections.Generic;

namespace Spreadsheet.Dto.Spreadsheet
{
    public class SpreadsheetMetadataDto
    {
        public long Id { get; set; }
        public string SpreadsheetId { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual ICollection<SpreadsheetTabMetadataDto> SpreadsheetTabMetadataDtos { get; set; }
    }
}