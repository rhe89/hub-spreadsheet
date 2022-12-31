using System.Threading.Tasks;
using Hub.Shared.DataContracts.Spreadsheet.Query;
using Microsoft.AspNetCore.Mvc;
using Spreadsheet.Providers;

namespace Spreadsheet.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DebtController : ControllerBase
{
    private readonly IDebtTabProvider _debtTabProvider;

    public DebtController(IDebtTabProvider debtTabProvider)
    {
        _debtTabProvider = debtTabProvider;
    }

    [HttpPost]
    public async Task<IActionResult> Get(DebtQuery query)
    {
        var debtList = await _debtTabProvider.GetDebtList(query);

        return Ok(debtList);
    }
}