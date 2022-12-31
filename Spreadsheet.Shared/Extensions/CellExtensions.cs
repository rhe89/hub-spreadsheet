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
            throw new ArgumentException(nameof(dateString), $"Invalid format: Excepted dd.MM.yyyy, actual '{dateString}'");
        }

        var dayString = dateParts[0];
        var monthString = dateParts[1];
        var yearString = dateParts[2];

        if (!int.TryParse(dayString, out var day) ||
            !int.TryParse(monthString, out var month) ||
            !int.TryParse(yearString, out var year))
        {
            throw new ArgumentException(nameof(dateString), $"Invalid format: Excepted dd.MM.yyyy, actual '{dateString}'");
        }

        return new DateTime(year, month, day);
    }
    
    public static decimal ParseNumberInCell(this object numberCell)
    {
        var numberString = numberCell.ToString();

        if (string.IsNullOrWhiteSpace(numberString))
        {
            return 0;
        }

        var numberStringStripped = numberString
            .Replace("kr", "")
            .Replace(" ", "");

        if (!decimal.TryParse(numberStringStripped, out var number))
        {
            throw new ArgumentException(nameof(numberStringStripped), $"Invalid format: '{numberString}' (Result was {numberStringStripped})");
        }

        return number;
    }
}