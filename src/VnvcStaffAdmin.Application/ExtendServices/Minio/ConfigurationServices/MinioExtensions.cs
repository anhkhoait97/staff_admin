using Elastic.Clients.Elasticsearch.TransformManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.DataModel.Args;
using Newtonsoft.Json;
using VnvcStaffAdmin.Domain.SettingModel;

namespace VnvcStaffAdmin.Application.ExtendServices.Minio.ConfigurationServices
{
    public static class MinioExtensions
    {
        public static IServiceCollection AddMinioService(this IServiceCollection services)
        {
            var settings = new MinioSettings
            {
                Endpoint = Environment.GetEnvironmentVariable("S3_MINIO_ENDPOINT"),
                AccessKey = Environment.GetEnvironmentVariable("S3_MINIO_ACCESSKEY"),
                SecretKey = Environment.GetEnvironmentVariable("S3_MINIO_SECRETKEY"),
                Region = Environment.GetEnvironmentVariable("S3_MINIO_REGION"),
                Secure = true,
            };

            services.AddSingleton<IMinioClient>(serviceProvider =>
            {
                return new MinioClient()
                    .WithEndpoint(settings.Endpoint)
                    .WithCredentials(settings.AccessKey, settings.SecretKey)
                    .WithRegion(settings.Region)
                    .WithSSL(settings.Secure)
                    .Build();
            });

            services.AddSingleton(settings);
            services.AddScoped<IMinioService, MinioService>();

            return services;
        }
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
                    .WithBucket(minioSettings?.BucketName);

                bool found = await minioClient.BucketExistsAsync(bucketExistsArgs);
                if (!found)
                {
                    // Make a new bucket
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(minioSettings?.BucketName);

                    await minioClient.MakeBucketAsync(makeBucketArgs);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}