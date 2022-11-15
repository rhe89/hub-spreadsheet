using System.Collections.Generic;
using System.Threading.Tasks;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Providers;

public interface ITabDataProvider<TTab> 
    where TTab : Tab
{
    public Task<IEnumerable<ICell>> GetData(string messageBody);
}