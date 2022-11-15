using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Shared.DataContracts.Crypto.Query;
using Hub.Shared.Web.Http;
using JetBrains.Annotations;
using Spreadsheet.Integration.Dto;

namespace Spreadsheet.Integration;

public interface ICryptoApiConnector
{
    public string FriendlyApiName { get; }
    Task<IList<CryptoAccountCell>> GetAccounts();
    Task<IList<ExchangeRateCell>> GetExchangeRates();
}

[UsedImplicitly]
public class CryptoApiConnector : HttpClientService, ICryptoApiConnector
{
    private const string AccountsPath = "/api/accounts";
    private const string ExchangeRatesPath = "/api/exchangerates";

    public CryptoApiConnector(HttpClient httpClient) : base(httpClient, "CryptoApi")
    {
    }
    
    public async Task<IList<CryptoAccountCell>> GetAccounts()
    {
        return await Post<IList<CryptoAccountCell>>(AccountsPath, new AccountQuery { MergeAccountsWithSameNameFromDifferentExchanges = true});
    }

    public async Task<IList<ExchangeRateCell>> GetExchangeRates()
    {
        return await Post<IList<ExchangeRateCell>>(ExchangeRatesPath, new ExchangeRateQuery());
    }
}