using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spreadsheet.Integration;
using Spreadsheet.Integration.Dto.Spreadsheet;

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

        public async Task<IEnumerable<ICell>> GetData()
        {
            _logger.LogInformation("Getting accounts from {ApiName}", _bankApiConnector.FriendlyApiName);

            var bankAccounts = await _bankApiConnector.GetAccounts();

            _logger.LogInformation("Got {Count} accounts from {ApiName}", bankAccounts.Count,
                _bankApiConnector.FriendlyApiName);

            return bankAccounts;
        }
    }
}