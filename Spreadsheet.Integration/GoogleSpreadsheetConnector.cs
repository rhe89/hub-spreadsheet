using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Hub.Storage.Core.Providers;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Exceptions;
using Spreadsheet.Core.Integration;
using Spreadsheet.Core.Storage;

namespace Spreadsheet.Integration
{
    public class GoogleSpreadsheetConnector : IGoogleSpreadsheetConnector
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IAzureStorage _azureStorage;
        private readonly ILogger<GoogleSpreadsheetConnector> _logger;
        private readonly string _applicationName;

        public GoogleSpreadsheetConnector(ISettingProvider settingProvider, 
            IAzureStorage azureStorage, 
            ILogger<GoogleSpreadsheetConnector> logger)
        {
            _settingProvider = settingProvider;
            _azureStorage = azureStorage;
            _logger = logger;
            _applicationName = "Hub";
        }
        
        public async Task UpdateSpreadsheetTab(TabDtoBase tabDtoBase, int firstColumnRow, int lastColumnRow)
        {
            try
            {
                var range =  GetRangeFormatted(tabDtoBase.Name,
                tabDtoBase.FirstColumn,
                firstColumnRow,
                tabDtoBase.LastColumn,
                lastColumnRow);
            
                var updatedValues = new ValueRange
                {
                    MajorDimension = "ROWS",
                    Values = tabDtoBase.Rows.Select(row => row.Cells).ToList(),
                    Range = range
                };

                var sheetsService = await GetSheetsService();

                var update = sheetsService.Spreadsheets.Values.Update(updatedValues, tabDtoBase.SpreadsheetId, range);
                
                update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

                await update.ExecuteAsync();
            }
            catch (Exception e)
            {
                throw new SpreadsheetConnectorException($"Error updating tab {tabDtoBase.Name} in spreadsheet with id {tabDtoBase.SpreadsheetId} from Google API", e);
            }
        }
        
        public async Task LoadSpreadsheetTab(TabDtoBase tabDtoBase)
        {
            try
            {
                _logger.LogInformation($"Getting tab {tabDtoBase.Name} in sheet with id {tabDtoBase.SpreadsheetId} from Google API");

                var request = await GetSpreadsheetRequest(tabDtoBase);

                var response = await request.ExecuteAsync();
                
                tabDtoBase.PopulateRows(response.Values);
            }
            catch (Exception e)
            {
                throw new SpreadsheetConnectorException($"Error getting tab {tabDtoBase.Name} in sheet with id {tabDtoBase.SpreadsheetId} from Google API", e);
            }
        }
        
        private async Task<SpreadsheetsResource.ValuesResource.GetRequest> GetSpreadsheetRequest(TabDtoBase tabDtoBase)
        {
            var range = GetRangeFormatted(tabDtoBase.Name, tabDtoBase.FirstColumn, tabDtoBase.LastColumn);

            var sheetsService = await GetSheetsService();

            return sheetsService.Spreadsheets.Values.Get(tabDtoBase.SpreadsheetId, range);
        }

        private static string GetRangeFormatted(string tabName, string firstColumn, string lastColumn)
        {
            return $"{tabName}!{firstColumn}:{lastColumn}";
        }

        private static string GetRangeFormatted(string tabName, string firstColumn, int firstColumnRow, string lastColumn, int lastColumnRow)
        {
            return $"{tabName}!{firstColumn}{firstColumnRow}:{lastColumn}{lastColumnRow}";
        }
        
        private async Task<SheetsService> GetSheetsService()
        {
            var serverCredentials = await GetServiceAccountCredential();

            if (serverCredentials == null)
            {
                throw new SpreadsheetConnectorException("Getting Google service account credentials failed");
            }

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
        
        private async Task<ServiceAccountCredential> GetServiceAccountCredential()
        {
            var keyFile = await _azureStorage.GetGoogleCertificate();
            var privateKey = _settingProvider.GetSetting<string>(SettingConstants.GooglePrivateKey);
            var serviceAccountEmail = _settingProvider.GetSetting<string>(SettingConstants.GoogleServiceAccountEmail);

            var certificate = new X509Certificate2(keyFile,
                $"{privateKey}",
                X509KeyStorageFlags.MachineKeySet |
                X509KeyStorageFlags.PersistKeySet |
                X509KeyStorageFlags.Exportable);

            try
            {
                var accountCredential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = new[] { SheetsService.Scope.Spreadsheets }
                    }.FromCertificate(certificate));

                return accountCredential;
            }
            catch (Exception e)
            {
                throw new SpreadsheetConnectorException("Error when authenticating ServiceAccountCredentials: ", e);
            }
        }
    }
}