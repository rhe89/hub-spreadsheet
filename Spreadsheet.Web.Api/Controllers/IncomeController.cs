using System.Threading.Tasks;
using Hub.Shared.DataContracts.Spreadsheet.Query;
using Microsoft.AspNetCore.Mvc;
using Spreadsheet.Providers;

namespace Spreadsheet.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncomeController : ControllerBase
{
    private readonly IIncomeTabProvider _incomeTabProvider;

    public IncomeController(IIncomeTabProvider incomeTabProvider)
    {
        _incomeTabProvider = incomeTabProvider;
    }

    [HttpPost]
    public async Task<IActionResult> Get(IncomeQuery incomeQuery)
    {
        var incomeList = await _incomeTabProvider.GetIncomeList(incomeQuery);

        return Ok(incomeList);
    }
}