using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Spreadsheet.Integration.Dto;

namespace Spreadsheet.Integration
{
    public interface ISbankenApiConnector : IBankApiConnector
    {
        Task<IList<TransactionDto>> GetBillingAccountTransactions(int ageInDays);
    }

    [UsedImplicitly]
    public class SbankenApiConnector : BankApiConnector, ISbankenApiConnector
    {
        private const string TransactionsPath = "/api/transactions";

        public SbankenApiConnector(HttpClient httpClient) : base(httpClient, "SbankenApi")
        {
        }

        public async Task<IList<TransactionDto>> GetBillingAccountTransactions(int ageInDays)
        {
            return await Get<IList<TransactionDto>>(TransactionsPath,
                $"ageInDays={ageInDays}&accountName=Regningsbetaling");
        }
    }
}