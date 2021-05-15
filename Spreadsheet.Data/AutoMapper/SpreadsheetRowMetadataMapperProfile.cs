using AutoMapper;
using Spreadsheet.Core.Dto.Data;
using Spreadsheet.Core.Entities;

namespace Spreadsheet.Data.AutoMapper
{
    public class SpreadsheetRowMetadataMapperProfile : Profile
    {
        public SpreadsheetRowMetadataMapperProfile()
        {
            CreateMap<SpreadsheetRowMetadata, SpreadsheetRowMetadataDto>();
        }
    }
}