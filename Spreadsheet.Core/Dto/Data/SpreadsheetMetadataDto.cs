using System;
using System.Collections.Generic;
using Hub.Storage.Core.Dto;
using Spreadsheet.Core.Dto.Spreadsheet;

namespace Spreadsheet.Core.Dto.Data
{
    public class SpreadsheetMetadataDto : EntityDtoBase
    {
        public string SpreadsheetId { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual ICollection<SpreadsheetTabMetadataDto> SpreadsheetTabMetadataDtos { get; set; }
    }
}