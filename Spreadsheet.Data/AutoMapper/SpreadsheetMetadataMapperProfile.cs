using AutoMapper;
using Spreadsheet.Data.Documents;
using Spreadsheet.Data.Dto;

namespace Spreadsheet.Data.AutoMapper;

public class SpreadsheetMetadataMapperProfile : Profile
{
    public SpreadsheetMetadataMapperProfile()
    {
        CreateMap<SpreadsheetMetadata, SpreadsheetMetadataDto>()
            .ReverseMap();
    }
}