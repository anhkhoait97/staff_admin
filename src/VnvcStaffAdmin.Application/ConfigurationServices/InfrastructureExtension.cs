using Microsoft.Extensions.DependencyInjection;
using VnvcStaffAdmin.Infrastructure;
using VnvcStaffAdmin.Infrastructure.Context;
using VnvcStaffAdmin.Infrastructure.Interface;
using VnvcStaffAdmin.Infrastructure.Interface.DbContext;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;
using VnvcStaffAdmin.Infrastructure.Uow;

namespace VnvcStaffAdmin.Application.ConfigurationServices
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IVnvcStaffContext, VnvcStaffContext>();
            services.AddScoped<IVnvcContext, VnvcContext>();
            services.AddScoped<IVnvcUow, VnvcUow>();
            services.AddScoped<IVnvcStaffUow, VnvcStaffUow>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            return services;
        }
    }
}