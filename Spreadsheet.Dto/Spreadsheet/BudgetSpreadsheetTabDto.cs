using System;
using System.Linq;

namespace Spreadsheet.Dto.Spreadsheet
{
    public class BudgetSpreadsheetTabDto : TabDto
    {
        public BudgetSpreadsheetTabDto()
        {
            PopulateAllRows = true;
        }
        
        public int GetColIndexOfCurrentPeriodInSheet()
        {
            var currentPeriod = GetCurrentPeriod();

            return GetColIndexOfPeriodInSheet(currentPeriod);
        }
        
        public int GetColIndexOfNextPeriodInSheet()
        {
            var nextPeriod = GetNextPeriod();

            return GetColIndexOfPeriodInSheet(nextPeriod);
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

        public static string GetCurrentPeriod()
        {
            return $"{DateTime.Now.Month}/{DateTime.Now.Year}";
        }
        
        private static string GetNextPeriod()
        {
            var month = DateTime.Now.Month;

            return month == 12 ? 
                $"01/{DateTime.Now.Year+1}" : 
                $"{DateTime.Now.Month+1}/{DateTime.Now.Year}";
        }
    }
}
