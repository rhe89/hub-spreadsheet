using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Integration;

namespace Spreadsheet.Core.SpreadsheetTabWriters
{
    public interface IExchangeRatesTabWriter
    {
        Task UpdateTab(IList<ExchangeRateDto> accounts, DateTime bankAccountTaskLastRun);
    }
}