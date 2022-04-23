using AutoMapper;

namespace Spreadsheet.Data.AutoMapper;

public static class MapperConfigurationExpressionExtensions
{
    public static void AddSpreadsheetProfiles(this IMapperConfigurationExpression mapperConfigurationExpression)
    {
        mapperConfigurationExpression.AddProfile<SpreadsheetMetadataMapperProfile>();
    }
}