using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Providers;
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
        
        [HttpPost("QueueUpdateCoinbaseAccountsTask")]
        public IActionResult QueueUpdateCoinbaseAccountsTask()
        {
            _backgroundTaskQueueHandler.QueueBackgroundTask<UpdateCoinbaseAccountsTask>();

            return Ok();
        }
        
        [HttpPost("QueueUpdateSbankenAccountsTask")]
        public IActionResult QueueUpdateSbankenAccountsTask()
        {
            _backgroundTaskQueueHandler.QueueBackgroundTask<UpdateSbankenAccountsTask>();

            return Ok();
        }
        
        [HttpPost("QueueUpdateCoinbaseProAccountsTask")]
        public IActionResult QueueUpdateCoinbaseProAccountsTask()
        {
            _backgroundTaskQueueHandler.QueueBackgroundTask<UpdateCoinbaseProAccountsTask>();

            return Ok();
        }
        
        [HttpPost("QueueUpdateExchangeRatesTask")]
        public IActionResult QueueUpdateExchangeRatesTask()
        {
            _backgroundTaskQueueHandler.QueueBackgroundTask<UpdateExchangeRatesTask>();

            return Ok();
        }
        
        [HttpPost("QueueUpdateBillingAccountPaymentsTask")]
        public IActionResult QueueUpdateBillingAccountPaymentsTask()
        {
            _backgroundTaskQueueHandler.QueueBackgroundTask<UpdateBillingAccountPaymentsTask>();

            return Ok();
        }
    }
}