using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.SpreadsheetTabWriters;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateSbankenAccountsTask : UpdateBankAccountTaskBase<SbankenAccountsTabDto>
    {
        public UpdateSbankenAccountsTask(ILogger<UpdateSbankenAccountsTask> logger, 
            ISbankenApiConnector sbankenApiConnector,
            IBankAccountsTabWriter<SbankenAccountsTabDto> sbankenAccountsTabWriter, 
            IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory) : 
                base(backgroundTaskConfigurationProvider, 
                     backgroundTaskConfigurationFactory, 
                     sbankenAccountsTabWriter,
                     sbankenApiConnector,
                     logger)
        { }
    }
}