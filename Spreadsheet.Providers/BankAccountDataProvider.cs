using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Providers;

namespace Spreadsheet.Providers
{
    public class BankAccountTabDataProvider<TTab, TBankApiConnector> : ITabDataProvider<TTab> 
        where TTab : Tab
        where TBankApiConnector : IBankApiConnector
    {
        private readonly TBankApiConnector _bankApiConnector;
        private readonly ILogger<BankAccountTabDataProvider<TTab, TBankApiConnector>> _logger;

        public BankAccountTabDataProvider(TBankApiConnector bankApiConnector,
            ILogger<BankAccountTabDataProvider<TTab, TBankApiConnector>> logger)
        {
            _bankApiConnector = bankApiConnector;
            _logger = logger;
        }
        
        public async Task<IEnumerable<Cell>> GetData()
        {
            _logger.LogInformation($"Getting accounts from {_bankApiConnector.FriendlyApiName}");

            var response = await _bankApiConnector.GetAccounts();

            if (!response.Success)
            {
                throw new Exception(
                    $"GetData: {_bankApiConnector.FriendlyApiName}: {response.ErrorMessage}");
            }

            var bankAccounts = response.Data;
            
            _logger.LogInformation($"Got {bankAccounts.Count} accounts from {_bankApiConnector.FriendlyApiName}");

            return bankAccounts;
        }

        public async Task<DateTime?> GetDataLastUpdated()
        {
            var response = await _bankApiConnector.GetBackgroundTaskConfigurations();

            if (!response.Success)
            {
                throw new Exception(
                    $"GetDataLastUpdated: {_bankApiConnector.FriendlyApiName}: {response.ErrorMessage}");
            }
            
            var bankAccountsTask = response.Data
                ?.FirstOrDefault(x => x.Name == "UpdateAccountsTask");

            return bankAccountsTask?.LastRun;
        }
    }
}