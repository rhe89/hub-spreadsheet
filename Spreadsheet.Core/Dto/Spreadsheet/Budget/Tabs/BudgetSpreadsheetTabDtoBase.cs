using System;
using System.Linq;

namespace Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs
{
    public abstract class BudgetSpreadsheetTabDtoBase : TabDtoBase
    {
        protected BudgetSpreadsheetTabDtoBase()
        {
            PopulateAllRows = true;
        }
        
        public int GetColIndexOfCurrentPeriodInSheet()
        {
            var currentPeriod = GetCurrentPeriod();

            return GetColIndexOfPeriodInSheet(currentPeriod);
        }
        
        private int GetColIndexOfPeriodInSheet(string period)
        {
            var idx = 0;

            var periodRow = Rows.First();

            foreach (var col in periodRow.Cells)
            {
                if (col.ToString() == period)
                {
                    return idx;
                }

                idx++;
            }

            return -1;
        }

        public string GetCurrentPeriod()
        {
            return $"{DateTime.Now.Month}/{DateTime.Now.Year}";
        }
    }
}
