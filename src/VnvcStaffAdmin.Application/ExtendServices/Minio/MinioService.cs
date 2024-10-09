using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Newtonsoft.Json;
using VnvcStaffAdmin.Domain.SettingModel;

namespace VnvcStaffAdmin.Application.ExtendServices.Minio
{
    public class MinioService : IMinioService
    {
        private readonly string? _bucketName;
        private readonly ILogger<MinioService> _logger;
        private readonly IMinioClient _minioClient;

        public MinioService(
            ILogger<MinioService> logger,
            IMinioClient minioClient
        )
        {
            _logger = logger;
            _bucketName = Environment.GetEnvironmentVariable("S3_MINIO_BUCKETNAME");
            _minioClient = minioClient;
        }

        public async Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType)
        {
            try
            {
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length)
                    .WithContentType(contentType);

                await _minioClient.PutObjectAsync(putObjectArgs);
                _logger.LogInformation("File '{FileName}' uploaded successfully.", fileName);
                return await GeneratePreSignedURLAsync(fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file '{FileName}'", fileName);
                throw;
            }
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            try
            {
                var memoryStream = new MemoryStream();
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithCallbackStream(stream =>
                    {
                        stream.CopyTo(memoryStream);
                        _logger.LogInformation("Callback stream executed for file '{FileName}'.", fileName);
                    });

                await _minioClient.GetObjectAsync(getObjectArgs);
                memoryStream.Position = 0;

                _logger.LogInformation("File '{FileName}' downloaded successfully.", fileName);
                return memoryStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file '{FileName}'", fileName);
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs);
                _logger.LogInformation("File '{FileName}' deleted successfully.", fileName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file '{FileName}'", fileName);
                return false;
            }
        }

        public async Task<IEnumerable<string>> ListFilesAsync()
        {
            var fileNames = new List<string>();
            try
            {
                var listObjectsArgs = new ListObjectsArgs()
                    .WithBucket(_bucketName);

                var objects = _minioClient.ListObjectsEnumAsync(listObjectsArgs);
                await foreach (var item in objects)
                {
                    fileNames.Add(item.Key);
                }
                _logger.LogInformation("Files listed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing files");
            }
            return fileNames;
        }

        public async Task<object> GetFileMetadataAsync(string fileName)
        {
            try
            {
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName);

                var objectStat = await _minioClient.StatObjectAsync(statObjectArgs);
                _logger.LogInformation("Metadata for file '{FileName}' retrieved successfully.", fileName);

                return new
                {
                    objectStat.ObjectName,
                    objectStat.Size,
                    objectStat.LastModified,
                    objectStat.ContentType
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving metadata for file '{FileName}'", fileName);
                throw;
            }
        }

        public async Task<string> GeneratePreSignedURLAsync(string fileName)
        {
            try
            {
                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithExpiry(60 * 60); // 1 hour expiration

                var url = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
                _logger.LogInformation("Pre-signed URL for file '{FileName}' generated successfully.", fileName);
                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating pre-signed URL for file '{FileName}'", fileName);
                throw;
            }
        }
    }
}