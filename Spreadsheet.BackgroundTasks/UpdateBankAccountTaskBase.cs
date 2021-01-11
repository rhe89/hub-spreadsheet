using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.BackgroundTasks;
using Spreadsheet.Core.Dto.Spreadsheet.Budget.Tabs;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.SpreadsheetTabWriters;

namespace Spreadsheet.BackgroundTasks
{
    public abstract class UpdateBankAccountTaskBase<TBankAccountsTabDto> : BackgroundTask
        where TBankAccountsTabDto : BankAccountsTabDto, new()
    {
        private readonly IBankAccountsTabWriter<TBankAccountsTabDto> _bankAccountsTabWriter;
        private readonly IBankApiConnector _bankApiConnector;
        private readonly ILogger<UpdateBankAccountTaskBase<TBankAccountsTabDto>> _logger;

        protected UpdateBankAccountTaskBase(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider, 
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            IBankAccountsTabWriter<TBankAccountsTabDto> bankAccountsTabWriter,
            IBankApiConnector bankApiConnector,
            ILogger<UpdateBankAccountTaskBase<TBankAccountsTabDto>> logger) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _bankAccountsTabWriter = bankAccountsTabWriter;
            _bankApiConnector = bankApiConnector;
            _logger = logger;
        }
        
        public override async Task Execute(CancellationToken cancellationToken)
        {
            var bankAccounts = await GetBankAccounts();

            if (bankAccounts == null)
            {
                return;
            }

            var bankAccountsTaskLastRun = await GetBankAccountsTaskLastRun();

            await _bankAccountsTabWriter.UpdateTab(bankAccounts, bankAccountsTaskLastRun);
        }


        private async Task<IList<AccountDto>> GetBankAccounts()
        {
            _logger.LogInformation($"Getting accounts from {_bankApiConnector.FriendlyApiName}");

            var response = await _bankApiConnector.GetAccounts();

            if (!response.Success)
            {
                throw new Exception(
                    $"GetBankAccounts: {_bankApiConnector.FriendlyApiName}: {response.ErrorMessage}");
            }

            var bankAccounts = response.Data;
            
            _logger.LogInformation($"Got {bankAccounts.Count} accounts from {_bankApiConnector.FriendlyApiName}");

            return bankAccounts;
        }
        
        private async Task<DateTime> GetBankAccountsTaskLastRun()
        {
            var response = await _bankApiConnector.GetBackgroundTaskConfigurations();

            if (!response.Success)
            {
                throw new Exception(
                    $"GetBankAccountsTaskLastRun: {_bankApiConnector.FriendlyApiName}: {response.ErrorMessage}");
            }
            
            var bankAccountsTask = response.Data
                ?.FirstOrDefault(x => x.Name == "UpdateAccountsTask");

            return bankAccountsTask?.LastRun ?? DateTime.MinValue;
        }
    }
}