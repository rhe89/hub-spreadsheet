using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Shared.DataContracts.Banking.Dto;
using Hub.Shared.DataContracts.Banking.Query;
using Hub.Shared.Web.Http;
using JetBrains.Annotations;
using Spreadsheet.Integration.Dto;

namespace Spreadsheet.Integration;

public interface IBankingApiConnector
{
    public string FriendlyApiName { get; }
    Task<IList<BankingAccountBalanceCell>> GetAccountBalances(AccountQuery accountQuery);
    Task<IList<TransactionDto>> GetTransactions(TransactionQuery transactionQuery);
    Task<IList<ScheduledTransactionDto>> GetScheduledTransactions(ScheduledTransactionQuery scheduledTransactionQuery);
}

[UsedImplicitly]
public class BankingApiConnector : HttpClientService, IBankingApiConnector
{
    private const string AccountsPath = "/api/accounts";
    private const string TransactionsPath = "/api/transactions";
    private const string ScheduledTransactionsPath = "/api/scheduledtransactions";

    public BankingApiConnector(HttpClient httpClient) : base(httpClient, "BankingApi")
    {
    }
    
    public async Task<IList<BankingAccountBalanceCell>> GetAccountBalances(AccountQuery accountQuery)
    {
        return await Post<IList<BankingAccountBalanceCell>>(AccountsPath, accountQuery);
    }

    public async Task<IList<TransactionDto>> GetTransactions(TransactionQuery transactionQuery)
    {
        return await Post<IList<TransactionDto>>(TransactionsPath, transactionQuery);
    }

    public async Task<IList<ScheduledTransactionDto>> GetScheduledTransactions(ScheduledTransactionQuery scheduledTransactionQuery)
    {
        return await Post<IList<ScheduledTransactionDto>>(ScheduledTransactionsPath, scheduledTransactionQuery);
    }
}