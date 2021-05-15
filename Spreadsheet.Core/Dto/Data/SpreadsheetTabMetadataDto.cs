using System.Collections.Generic;
using Hub.Storage.Repository.Dto;

namespace Spreadsheet.Core.Dto.Data
{
    public class SpreadsheetTabMetadataDto : EntityDtoBase
    {
        public long SpreadsheetMetadataId { get; set; }
        public string Name { get; set; }
        public string FirstColumn { get; set; }
        public string LastColumn { get; set; }
        
        public IList<SpreadsheetRowMetadataDto> SpreadsheetRowMetadata { get; set; }
    }
}