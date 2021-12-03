using System;
using Microsoft.AspNetCore.Components;

namespace Spreadsheet.Web.WebApp.Helpers
{
    public static class InputFormatter
    {
        public static string ToUpperCase(ChangeEventArgs changeEventArgs)
        {
            if (!(changeEventArgs.Value is string text))
                throw new ArgumentException("Input type was not string", nameof(changeEventArgs.Value));

            changeEventArgs.Value = text.ToUpper();
            
            return Trim(changeEventArgs);
        }

        public static string Trim(ChangeEventArgs changeEventArgs)
        {
            if (!(changeEventArgs.Value is string text))
                throw new ArgumentException("Input type was not string", nameof(changeEventArgs.Value));
            
            return text.Replace(" ", "");
        }
    }
}