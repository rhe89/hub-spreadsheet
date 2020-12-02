using AutoMapper;

namespace Spreadsheet.Data.AutoMapper
{
    public static class MapperConfigurationExpressionExtensions
    {
        public static IMapperConfigurationExpression AddSpreadsheetProfiles(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<SpreadsheetMetadataMapperProfile>();
            mapperConfigurationExpression.AddProfile<SpreadsheetRowMetadataMapperProfile>();
            mapperConfigurationExpression.AddProfile<SpreadsheetTabMetadataMapperProfile>();

            return mapperConfigurationExpression;
        }
    }
}