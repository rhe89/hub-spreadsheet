using System;
using System.Threading.Tasks;
using Hub.Storage.Providers;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using Spreadsheet.Shared.Constants;

namespace Spreadsheet.Data.Storage
{
    public class AzureStorage : IAzureStorage
    {
        private readonly ISettingProvider _settingProvider;

        public AzureStorage(ISettingProvider settingProvider)
        {
            _settingProvider = settingProvider;
        }

        public CloudFileDirectory GetCertificateDirectory()
        {
            var storageAccount = CloudStorageAccount.Parse(_settingProvider.GetSetting<string>(SettingConstants.StorageAccount));

            var fileClient = storageAccount.CreateCloudFileClient();

            var share = fileClient.GetShareReference(_settingProvider.GetSetting<string>(SettingConstants.StorageAccountFileShare));

            var rootDir = share.GetRootDirectoryReference();

            var certificateDir = rootDir.GetDirectoryReference(_settingProvider.GetSetting<string>(SettingConstants.StorageAccountFileShareCertificateFolder));

            return certificateDir;
        }

        public async Task<byte[]> GetGoogleCertificate()
        {
            var certificateDir = GetCertificateDirectory();
            
            var file = certificateDir.GetFileReference(_settingProvider.GetSetting<string>(SettingConstants.GoogleCertificate));
            var byteArr = new byte[file.StreamMinimumReadSizeInBytes];

            await file.DownloadToByteArrayAsync(byteArr, 0);

            return byteArr;
        }
    }
}