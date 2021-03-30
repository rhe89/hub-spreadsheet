using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Providers;
using Spreadsheet.Core.Services;

namespace Spreadsheet.BackgroundTasks
{
    public abstract class UpdateTabTaskBase<TTab> : BackgroundTask
        where TTab : Tab, new()
    {
        private readonly ITabDataProvider<TTab> _tabDataProvider;
        private readonly ITabWriterService<TTab> _tabWriterService;

        protected UpdateTabTaskBase(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider, 
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            ITabDataProvider<TTab> tabDataProvider,
            ITabWriterService<TTab> tabWriterService) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _tabDataProvider = tabDataProvider;
            _tabWriterService = tabWriterService;
        }
        
        public override async Task Execute(CancellationToken cancellationToken)
        {
            var data = await _tabDataProvider.GetData();

            if (data == null)
            {
                return;
            }

            var dataLastUpdated = await _tabDataProvider.GetDataLastUpdated();

            await _tabWriterService.UpdateTab(data, dataLastUpdated);
        }
    }
}