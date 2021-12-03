using System;
using FluentValidation;
using Spreadsheet.Data.Dto;

namespace Spreadsheet.Web.WebApp.Validation
{
    public class SpreadsheetMetadataValidator : AbstractValidator<SpreadsheetMetadataDto>
    {
        
        public SpreadsheetMetadataValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Required");
            
            RuleFor(x => x.SpreadsheetId)
                .NotEmpty().WithMessage("Required");
            
            RuleFor(x => x.Tabs)
                .NotEmpty().WithMessage("At least one tab is required");

            RuleForEach(x => x.Tabs).SetValidator(new TabValidator());
        }   
    }
    
    public class TabValidator : AbstractValidator<SpreadsheetMetadataDto.Tab>
    {
        private const string ValidFirstColumn = "A";
        private const string ValidLastColumn = "M"; 
        
        public TabValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Required");

            RuleFor(x => x.FirstColumn)
                .NotEmpty().WithMessage("Required")
                .Must(BeAValidSpreadsheetColumn).WithMessage($"Must be a string between {ValidFirstColumn} and {ValidLastColumn}");

            RuleFor(x => x.LastColumn)
                .NotEmpty().WithMessage("Required")
                .Must(BeAValidSpreadsheetColumn).WithMessage($"Must be a string between {ValidFirstColumn} and {ValidLastColumn}");
            
            RuleFor(x => x.Rows)
                .NotEmpty().WithMessage("At least one row is required");
            
            RuleForEach(x => x.Rows).SetValidator(new RowValidator());
        }

        private static bool BeAValidSpreadsheetColumn(string value)
        {
            return string.Compare(value, ValidFirstColumn, StringComparison.OrdinalIgnoreCase) >= 0 &&
                   string.Compare(value, ValidLastColumn, StringComparison.OrdinalIgnoreCase) <= 0;
        }
    }
    
    public class RowValidator : AbstractValidator<SpreadsheetMetadataDto.Row>
    {
        public RowValidator()
        {
            RuleFor(x => x.RowKey)                
                .NotEmpty().WithMessage("Required");
        }   
    }
    
    
}