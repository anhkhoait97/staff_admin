using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio.DataModel.Args;
using Minio;
using VnvcStaffAdmin.Domain.SettingModel;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace VnvcStaffAdmin.Application.ConfigurationServices
{
    public static class MinioExtension
    {
        public static async Task EnsureMiniBucketExists(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var minioClient = scope.ServiceProvider.GetRequiredService<IMinioClient>();
            var minioConfigJson = Environment.GetEnvironmentVariable("MINIO_SETTING") ?? "";
            var minioSettings = JsonConvert.DeserializeObject<MinioSettings>(minioConfigJson);

            try
            {
                // Check if bucket exists
                var bucketExistsArgs = new BucketExistsArgs()
                    .WithBucket(minioSettings.BucketName);

                bool found = await minioClient.BucketExistsAsync(bucketExistsArgs);
                if (!found)
                {
                    // Make a new bucket
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(minioSettings.BucketName);

                    await minioClient.MakeBucketAsync(makeBucketArgs);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
