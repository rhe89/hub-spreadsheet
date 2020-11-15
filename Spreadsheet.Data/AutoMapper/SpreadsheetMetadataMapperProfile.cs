using AutoMapper;
using Spreadsheet.Core.Dto.Data;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Entities;

namespace Spreadsheet.Data.AutoMapper
{
    public class SpreadsheetMetadataMapperProfile : Profile
    {
        public SpreadsheetMetadataMapperProfile()
        {
            CreateMap<SpreadsheetMetadata, SpreadsheetMetadataDto>();
        }
    }
}