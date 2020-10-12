using System.Threading.Tasks;
using Microsoft.Azure.Storage.File;

namespace Spreadsheet.Data.Storage
{
    public interface IAzureStorage
    {
        CloudFileDirectory GetCertificateDirectory();
        Task<byte[]> GetGoogleCertificate();
    }
}