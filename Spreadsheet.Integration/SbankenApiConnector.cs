using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Web.Http;
using Spreadsheet.Core.Dto.Integration;
using Spreadsheet.Core.Integration;

namespace Spreadsheet.Integration
{
    public class SbankenApiConnector : BankApiConnector, ISbankenApiConnector
    {
        private const string TransactionsPath = "/api/transactions";

        public SbankenApiConnector(HttpClient httpClient) : base(httpClient, "SbankenApi") {}

        public async Task<Response<IList<TransactionDto>>> GetBillingAccountTransactions()
        {
            return await Get<IList<TransactionDto>>(TransactionsPath, $"accountName=Regningsbetaling");
        }
    }
}