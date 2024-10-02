using Microsoft.Extensions.DependencyInjection;
using VnvcStaffAdmin.Application.Services;
using VnvcStaffAdmin.Application.Services.Interfaces;

namespace VnvcStaffAdmin.Application.ConfigurationServices
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IRecruitmentService, RecruitmentService>();
            services.AddScoped<ITermAndConditionService, TermAndConditionService>();
            services.AddScoped<IWorkSheetService, WorkSheetService>();

            return services;
        }
    }
}