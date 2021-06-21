using AutoMapper;
using Hub.Settings;

namespace Spreadsheet.Data.AutoMapper
{
    public static class MapperConfigurationExpressionExtensions
    {
        public static IMapperConfigurationExpression AddSpreadsheetProfiles(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<SpreadsheetMetadataMapperProfile>();
            mapperConfigurationExpression.AddProfile<SpreadsheetRowMetadataMapperProfile>();
            mapperConfigurationExpression.AddProfile<SpreadsheetTabMetadataMapperProfile>();
            mapperConfigurationExpression.AddProfile<BillingAccountTransactionMapperProfile>();

            return mapperConfigurationExpression;
        }
    }
}