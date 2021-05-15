using AutoMapper;
using Spreadsheet.Core.Dto.Data;
using Spreadsheet.Core.Entities;

namespace Spreadsheet.Data.AutoMapper
{
    public class SpreadsheetTabMetadataMapperProfile : Profile
    {
        public SpreadsheetTabMetadataMapperProfile()
        {
            CreateMap<SpreadsheetTabMetadata, SpreadsheetTabMetadataDto>();
        }
    }
}