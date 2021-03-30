using System.Globalization;

namespace Spreadsheet.Core.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToComma(this decimal number) 
        {
            return number.ToString(CultureInfo.CurrentCulture).Replace(".",",");
        }
    }
}