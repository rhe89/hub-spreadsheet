using System.Net.Http;
using JetBrains.Annotations;

namespace Spreadsheet.Integration;

public interface ICoinbaseProApiConnector : IBankApiConnector
{
}

[UsedImplicitly]
public class CoinbaseProApiConnector : BankApiConnector, ICoinbaseProApiConnector
{
    public CoinbaseProApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseProApi")
    {
    }
}