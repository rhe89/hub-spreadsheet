using System.Globalization;

namespace Spreadsheet.Shared.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToCommaString(this decimal number) 
        {
            return number.ToString(CultureInfo.CurrentCulture).Replace(".",",");
        }
    }
}