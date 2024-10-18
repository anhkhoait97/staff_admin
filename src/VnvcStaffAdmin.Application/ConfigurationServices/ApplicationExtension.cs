using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
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
            services.AddScoped<INewsCategoryService, NewsCategoryService>();

            return services;
        }
    }
}