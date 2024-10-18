using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.SettingModel;

namespace VnvcStaffAdmin.Application.ExtendServices.ElasticSearch.ConfigurationServices
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services)
        {
            var settings = new ElasticsearchSettings
            {
                Host = Environment.GetEnvironmentVariable("ES_HOST"),
                Port = Environment.GetEnvironmentVariable("ES_PORT"),
                UserName = Environment.GetEnvironmentVariable("ES_USER_NAME"),
                Password = Environment.GetEnvironmentVariable("ES_PASSWORD")
            };

            var uri = new Uri($"http://{settings.Host}:{settings.Port}");

            var connectionSettings = new ConnectionSettings(uri).DefaultIndex("default_index");

            if (!string.IsNullOrEmpty(settings.UserName) && !string.IsNullOrEmpty(settings.Password))
            {
                connectionSettings.BasicAuthentication(settings.UserName, settings.Password);
            }

            var client = new ElasticClient(connectionSettings);

            services.AddSingleton(settings);
            services.AddSingleton<IElasticClient>(client);
            services.AddScoped<IElasticsearchService, ElasticsearchService>();
        }
    }
}
