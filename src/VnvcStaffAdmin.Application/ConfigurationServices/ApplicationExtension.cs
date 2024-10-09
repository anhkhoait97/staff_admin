using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using Newtonsoft.Json;
using VnvcStaffAdmin.Application.ExtendServices.Minio;
using VnvcStaffAdmin.Application.Services;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.SettingModel;

namespace VnvcStaffAdmin.Application.ConfigurationServices
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IRecruitmentService, RecruitmentService>();
            services.AddScoped<ITermAndConditionService, TermAndConditionService>();
            services.AddScoped<IWorkSheetService, WorkSheetService>();
            services.AddScoped<IAppAccountService, AppAccountService>();
            services.AddScoped<INewsServices, NewsServices>();
            services.AddScoped<IBannerService, BannerService>();

            return services;
        }

        public static IServiceCollection AddMinioService(this IServiceCollection services, IConfiguration configuration)
        {
            var minioSettings = new MinioSettings
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
                    .WithEndpoint(minioSettings.Endpoint)
                    .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
                    .WithRegion(minioSettings.Region)
                    .WithSSL(minioSettings.Secure)
                    .Build();
            });

            services.AddScoped<IMinioService, MinioService>();

            return services;
        }
    }
}