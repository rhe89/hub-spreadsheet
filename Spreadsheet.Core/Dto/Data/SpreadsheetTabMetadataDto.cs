using System.Collections.Generic;
using Hub.Storage.Core.Dto;

namespace Spreadsheet.Core.Dto.Data
{
    public class SpreadsheetTabMetadataDto : EntityDtoBase
    {
        public string Name { get; set; }
        public string FirstColumn { get; set; }
        public string LastColumn { get; set; }
        
        public IList<SpreadsheetRowMetadataDto> SpreadsheetRowMetadataDtos { get; set; }
    }
}