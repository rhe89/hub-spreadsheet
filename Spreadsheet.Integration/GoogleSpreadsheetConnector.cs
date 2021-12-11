using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spreadsheet.Shared.Constants;
using Spreadsheet.Integration.Dto.Spreadsheet;

namespace Spreadsheet.Integration
{
    public interface IGoogleSpreadsheetConnector
    {
        Task LoadSpreadsheetTab(Tab tab);

        Task UpdateSpreadsheetTab(Tab tab, int firstColumnRow, int lastColumnRow);
    }

    public class GoogleSpreadsheetConnector : IGoogleSpreadsheetConnector
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GoogleSpreadsheetConnector> _logger;
        private readonly string _applicationName;

        public GoogleSpreadsheetConnector(IConfiguration configuration,
            ILogger<GoogleSpreadsheetConnector> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _applicationName = "Hub";
        }

        public async Task UpdateSpreadsheetTab(Tab tab, int firstColumnRow, int lastColumnRow)
        {
            var range = GetRangeFormatted(tab.Name,
                tab.FirstColumn,
                firstColumnRow,
                tab.LastColumn,
                lastColumnRow);

            var updatedValues = new ValueRange
            {
                MajorDimension = "ROWS",
                Values = tab.Rows.Select(row => row.Cells).ToList(),
                Range = range
            };

            var sheetsService = GetSheetsService();

            var update = sheetsService.Spreadsheets.Values.Update(updatedValues, tab.SpreadsheetId, range);

            update.ValueInputOption =
                SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

            await update.ExecuteAsync();
        }

        public async Task LoadSpreadsheetTab(Tab tab)
        {
            _logger.LogInformation("Getting tab {Tab} in sheet with id {Id} from Google API", tab.Name,
                tab.SpreadsheetId);

            var request = GetSpreadsheetRequest(tab);

            var response = await request.ExecuteAsync();

            tab.PopulateRows(response.Values);
        }

        private SpreadsheetsResource.ValuesResource.GetRequest GetSpreadsheetRequest(Tab tab)
        {
            var range = GetRangeFormatted(tab.Name, tab.FirstColumn, tab.LastColumn);

            var sheetsService = GetSheetsService();

            return sheetsService.Spreadsheets.Values.Get(tab.SpreadsheetId, range);
        }

        private static string GetRangeFormatted(string tabName, string firstColumn, string lastColumn)
        {
            return $"{tabName}!{firstColumn}:{lastColumn}";
        }

        private static string GetRangeFormatted(string tabName, string firstColumn, int firstColumnRow,
            string lastColumn, int lastColumnRow)
        {
            return $"{tabName}!{firstColumn}{firstColumnRow}:{lastColumn}{lastColumnRow}";
        }

        private SheetsService GetSheetsService()
        {
            var serverCredentials = GetServiceAccountCredential();

            if (serverCredentials == null)
                throw new SpreadsheetConnectorException("Getting Google service account credentials failed");

            try
            {
                var sheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = serverCredentials,
                    ApplicationName = _applicationName
                });
                return sheetsService;
            }
            catch (Exception e)
            {
                throw new SpreadsheetConnectorException("Error when initializing SheetsService", e);
            }
        }

        private ServiceAccountCredential GetServiceAccountCredential()
        {
            var googleCertificate = _configuration.GetValue<string>(SettingConstants.GoogleCertificate);
            var serviceAccountEmail = _configuration.GetValue<string>(SettingConstants.GoogleServiceAccountEmail);

            try
            {
                var certificate = new X509Certificate2(Convert.FromBase64String(googleCertificate),
                    string.Empty,
                    X509KeyStorageFlags.MachineKeySet |
                    X509KeyStorageFlags.PersistKeySet |
                    X509KeyStorageFlags.Exportable);

                var accountCredential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = new[] { SheetsService.Scope.Spreadsheets }
                    }.FromCertificate(certificate));

                return accountCredential;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed fetching service account credentials from Google");
                return null;
            }
        }

        private class SpreadsheetConnectorException : Exception
        {
            public SpreadsheetConnectorException(string message) : base(message)
            {
            }

            public SpreadsheetConnectorException(string message, Exception exception) : base(message, exception)
            {
            }
        }
    }
}