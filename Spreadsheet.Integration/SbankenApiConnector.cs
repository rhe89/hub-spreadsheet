using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Shared.Web.Http;
using Spreadsheet.Integration.Dto;

namespace Spreadsheet.Integration
{
    public interface ISbankenApiConnector : IBankApiConnector
    {
        Task<Response<IList<TransactionDto>>> GetBillingAccountTransactions(int ageInDays);
    }
    
    public class SbankenApiConnector : BankApiConnector, ISbankenApiConnector
    {
        private const string TransactionsPath = "/api/transactions";

        public SbankenApiConnector(HttpClient httpClient) : base(httpClient, "SbankenApi") {}

        public async Task<Response<IList<TransactionDto>>> GetBillingAccountTransactions(int ageInDays)
        {
            return await Get<IList<TransactionDto>>(TransactionsPath, $"ageInDays={ageInDays}&accountName=Regningsbetaling");
        }
    }
}