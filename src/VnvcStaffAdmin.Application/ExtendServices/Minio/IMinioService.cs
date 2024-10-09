using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Application.ExtendServices.Minio
{
    public interface IMinioService
    {
        Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType);
        Task<Stream> DownloadFileAsync(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
        Task<IEnumerable<string>> ListFilesAsync();
        Task<object> GetFileMetadataAsync(string fileName);
        Task<string> GeneratePreSignedURLAsync(string fileName);
    }
}
