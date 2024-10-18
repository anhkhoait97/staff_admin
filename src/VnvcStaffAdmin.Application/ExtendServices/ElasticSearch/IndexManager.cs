using Nest;
using System;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Application.ExtendServices.ElasticSearch
{
    public class IndexManager
    {
        private readonly IElasticClient _elasticClient;

        public IndexManager(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task EnsureIndexExistsAsync<T>(string indexName) where T : class
        {
            var existsResponse = await _elasticClient.Indices.ExistsAsync(indexName);

            if (!existsResponse.Exists)
            {
                var createIndexResponse = await _elasticClient.Indices.CreateAsync(indexName, c => c
                    .Map<T>(m => m.AutoMap())
                );

                if (!createIndexResponse.IsValid)
                {
                    throw new Exception($"Failed to create index '{indexName}': {createIndexResponse.OriginalException.Message}");
                }
            }
        }
    }
}