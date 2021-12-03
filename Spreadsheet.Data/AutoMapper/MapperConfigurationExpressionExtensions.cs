using AutoMapper;

namespace Spreadsheet.Data.AutoMapper
{
    public static class MapperConfigurationExpressionExtensions
    {
        public static IMapperConfigurationExpression AddSpreadsheetProfiles(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<SpreadsheetMetadataMapperProfile>();
            mapperConfigurationExpression.AddProfile<BillingAccountTransactionMapperProfile>();

            return mapperConfigurationExpression;
        }
    }
}