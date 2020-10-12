using System.Collections.Generic;

namespace Spreadsheet.Dto.Spreadsheet
{
    public class SpreadsheetTabMetadataDto
    {
        public string Name { get; set; }
        public string FirstColumn { get; set; }
        public string LastColumn { get; set; }
        
        public IList<SpreadsheetRowMetadataDto> SpreadsheetRowMetadataDtos { get; set; }
    }
}