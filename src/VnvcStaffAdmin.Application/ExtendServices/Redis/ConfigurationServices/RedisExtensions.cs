using Elastic.Clients.Elasticsearch.TransformManagement;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using VnvcStaffAdmin.Domain.SettingModel;

namespace VnvcStaffAdmin.Application.ExtendServices.Redis.ConfigurationServices
{
    public static class RedisExtensions
    {
        public static void AddRedis(this IServiceCollection services)
        {
            var settings = new RedisSettings
            {
                Host = Environment.GetEnvironmentVariable("REDIS_HOST"),
                Port = int.TryParse(Environment.GetEnvironmentVariable("REDIS_PORT"), out var port) ? port : 6379,
                Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD")
            };

            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { $"{settings.Host}:{settings.Port}" },
                Password = settings.Password,
                AbortOnConnectFail = false
            };

            var connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);

            services.AddSingleton(settings);
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
            services.AddScoped<IRedisService, RedisService>();
        }
    }
}