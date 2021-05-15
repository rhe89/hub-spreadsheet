using System.Collections.Generic;
using Hub.Storage.Repository.Dto;

namespace Spreadsheet.Core.Dto.Data
{
    public class SpreadsheetRowMetadataDto : EntityDtoBase
    {
        public string RowKey { get; set; }
        public string Tags { get; set; }

        public IEnumerable<string> TagList => Tags?.Split(",");
    }
}