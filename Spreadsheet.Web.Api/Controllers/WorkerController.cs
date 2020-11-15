using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Providers;
using Hub.Storage.Providers;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Spreadsheet.BackgroundTasks;

namespace Spreadsheet.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : WorkerControllerBase
    {
        private readonly IBackgroundTaskQueueHandler _backgroundTaskQueueHandler;

        public WorkerController(IWorkerLogProvider workerLogProvider,
            IBackgroundTaskQueueHandler backgroundTaskQueueHandler
        ) : base(workerLogProvider)
        {
            _backgroundTaskQueueHandler = backgroundTaskQueueHandler;
        }
        
        [HttpPost("QueueUpdateAccountTransfersTask")]
        public IActionResult QueueUpdateAccountTransfersTask()
        {
            _backgroundTaskQueueHandler.QueueBackgroundTask<UpdateAccountTransfersTask>();

            return Ok();
        }
        
        [HttpPost("QueueUpdateCoinbaseBalancesTask")]
        public IActionResult QueueUpdateCoinbaseBalancesTask()
        {
            _backgroundTaskQueueHandler.QueueBackgroundTask<UpdateCoinbaseBalancesTask>();

            return Ok();
        }
        
        [HttpPost("QueueUpdateSbankenBalancesTask")]
        public IActionResult QueueUpdateSbankenBalancesTask()
        {
            _backgroundTaskQueueHandler.QueueBackgroundTask<UpdateSbankenBalancesTask>();

            return Ok();
        }
    }
}