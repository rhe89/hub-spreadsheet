using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Hub.Settings.Core;
using Hub.Storage.Azure.Core;
using Microsoft.Extensions.Logging;
using Spreadsheet.Core.Constants;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Exceptions;
using Spreadsheet.Core.Integration;

namespace Spreadsheet.Integration
{
    public class GoogleSpreadsheetConnector : IGoogleSpreadsheetConnector
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IFileStorage _fileStorage;
        private readonly ILogger<GoogleSpreadsheetConnector> _logger;
        private readonly string _applicationName;

        public GoogleSpreadsheetConnector(ISettingProvider settingProvider, 
            IFileStorage fileStorage,
            ILogger<GoogleSpreadsheetConnector> logger)
        {
            _settingProvider = settingProvider;
            _fileStorage = fileStorage;
            _logger = logger;
            _applicationName = "Hub";
        }
        
        public async Task UpdateSpreadsheetTab(Tab tab, int firstColumnRow, int lastColumnRow)
        {
            var range =  GetRangeFormatted(tab.Name,
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

            var sheetsService = await GetSheetsService();

            var update = sheetsService.Spreadsheets.Values.Update(updatedValues, tab.SpreadsheetId, range);
            
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

            await update.ExecuteAsync();
        }
        
        public async Task LoadSpreadsheetTab(Tab tab)
        {
            _logger.LogInformation($"Getting tab {tab.Name} in sheet with id {tab.SpreadsheetId} from Google API");

            var request = await GetSpreadsheetRequest(tab);

            var response = await request.ExecuteAsync();
            
            tab.PopulateRows(response.Values);
        }
        
        private async Task<SpreadsheetsResource.ValuesResource.GetRequest> GetSpreadsheetRequest(Tab tab)
        {
            var range = GetRangeFormatted(tab.Name, tab.FirstColumn, tab.LastColumn);

            var sheetsService = await GetSheetsService();

            return sheetsService.Spreadsheets.Values.Get(tab.SpreadsheetId, range);
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
            var storageAccountFileShare = _settingProvider.GetSetting<string>(SettingConstants.StorageAccountFileShare);
            var storageAccountFileShareCertificateFolder = _settingProvider.GetSetting<string>(SettingConstants.StorageAccountFileShareCertificateFolder);
            var googleCertificateFileReference = _settingProvider.GetSetting<string>(SettingConstants.GoogleCertificate);
            var keyFile = await _fileStorage.GetItem(storageAccountFileShare, storageAccountFileShareCertificateFolder, googleCertificateFileReference);
            
            var privateKey = _settingProvider.GetSetting<string>(SettingConstants.GooglePrivateKey);
            var serviceAccountEmail = _settingProvider.GetSetting<string>(SettingConstants.GoogleServiceAccountEmail);

            var certificate = new X509Certificate2(keyFile,
                $"{privateKey}",
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
    }
}