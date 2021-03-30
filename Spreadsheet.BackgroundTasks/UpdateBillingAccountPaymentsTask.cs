using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.BackgroundTasks
{
    public class UpdateBillingAccountPaymentsTask : UpdateTabTaskBase<BillingAccountTab>
    {
        public UpdateBillingAccountPaymentsTask(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider, 
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            ITabWriterService<BillingAccountTab> tabWriterService,
            ITabDataProvider<BillingAccountTab> billingAccountPaymentsDataProvider) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory, billingAccountPaymentsDataProvider, tabWriterService)
        {
        }
    }
}