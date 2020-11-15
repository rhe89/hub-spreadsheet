using System;

namespace Spreadsheet.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime DateHour(this DateTime date) 
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
        }

        public static string FormattedDate(this DateTime date) 
        {
            var day = date.Day < 10 ? $"0{date.Day}" : $"{date.Day}";

            var month = date.Month < 10 ? $"0{date.Month}" : $"{date.Month}";

            var hour = date.Hour < 10 ? $"0{date.Hour}" : $"{date.Hour}";

            return $"{month}/{day}/{date.Year} {hour}:00:00";
        }

        public static string FormattedDateFull(this DateTime date) 
        {
            var day = date.Day < 10 ? $"0{date.Day}" : $"{date.Day}";

            var month = date.Month < 10 ? $"0{date.Month}" : $"{date.Month}";

            //Compensating for not bothering to fix timezone on server
            var hour = date.Hour < 10 ? $"0{date.Hour}" : $"{date.Hour}";
            var minute = date.Minute < 10 ? $"0{date.Minute}" : $"{date.Minute}";
            var seconds = date.Second < 10 ? $"0{date.Second}" : $"{date.Second}";

            return $"{month}/{day}/{date.Year} {hour}:{minute}:{seconds}";
        }
    }
}