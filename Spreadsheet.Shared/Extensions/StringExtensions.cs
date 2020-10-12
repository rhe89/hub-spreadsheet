using System;
using System.Globalization;

namespace Spreadsheet.Shared.Extensions
{
    public static class StringExtensions
    {
        public static DateTime DateTimeFormat(this string dateString) 
        {
            
            var date = DateTime.Parse(dateString, CultureInfo.InvariantCulture);

            return date.DateHour();
        }
    }
}