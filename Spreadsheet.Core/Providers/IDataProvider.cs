using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Dto.Spreadsheet;

namespace Spreadsheet.Core.Providers
{
    public interface ITabDataProvider<TTab> 
        where TTab : Tab
    {
        public Task<IEnumerable<Cell>> GetData();
        public Task<DateTime?> GetDataLastUpdated();
    }
}