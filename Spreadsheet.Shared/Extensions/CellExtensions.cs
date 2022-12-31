using System;

namespace Spreadsheet.Shared.Extensions;

public static class CellExtensions
{
    public static DateTime ParseDateStringInCell(this object dateCell)
    {
        var dateString = dateCell.ToString();

        if (string.IsNullOrWhiteSpace(dateString))
        {
            return DateTime.MinValue;
        }
        
        var dateParts = dateString.Split(".");

        if (dateParts.Length != 3)
        {
            throw new ArgumentException(nameof(dateString), $"Invalid format of date string. Excepted dd.MM.yyyy, actual {dateString}");
        }

        var dayString = dateParts[0];
        var monthString = dateParts[1];
        var yearString = dateParts[2];

        if (!int.TryParse(dayString, out var day) ||
            !int.TryParse(monthString, out var month) ||
            !int.TryParse(yearString, out var year))
        {
            throw new ArgumentException(nameof(dateString), $"Invalid format of date string. Excepted dd.MM.yyyy, actual {dateString}");
        }

        return new DateTime(year, month, day);
    }
    
    public static decimal ParseNumberInCell(this object dateCell)
    {
        var decimalString = dateCell.ToString();

        if (string.IsNullOrWhiteSpace(decimalString))
        {
            return 0;
        }

        var numberStringStripped = decimalString.Replace("kr", "").Replace(" ", "");

        if (!decimal.TryParse(numberStringStripped, out var number))
        {
            throw new ArgumentException(nameof(decimalString), $"Invalid format of number string: {decimalString}");
        }

        return number;
    }
}