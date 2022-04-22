using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Shared.DataContracts.Banking.SearchParameters;
using Hub.Shared.Web.Http;
using JetBrains.Annotations;
using Spreadsheet.Integration.Dto;

namespace Spreadsheet.Integration;

public interface IBankingApiConnector
{
    public string FriendlyApiName { get; }
    Task<IList<BankingAccountCell>> GetAccounts();
    Task<IList<TransactionCell>> GetTransactions(string accountType, int ageInDays);
}

[UsedImplicitly]
public class BankingApiConnector : HttpClientService, IBankingApiConnector
{
    private const string AccountsPath = "/api/accounts";
    private const string TransactionsPath = "/api/transactions";

    public BankingApiConnector(HttpClient httpClient) : base(httpClient, "BankingApi")
    {
    }
    
    public async Task<IList<BankingAccountCell>> GetAccounts()
    {
        return await Post<IList<BankingAccountCell>>(AccountsPath, new AccountSearchParameters { MergeAccountsWithSameNameFromDifferentBanks = true });
    }

    public async Task<IList<TransactionCell>> GetTransactions(string accountType, int ageInDays)
    {
        return await Post<IList<TransactionCell>>(TransactionsPath, new TransactionSearchParameters { AccountTypes = new [] { accountType }, FromDate = DateTime.Now.AddDays(-ageInDays)});
    }
}