using MongoDB.Driver;
using Nest;
using Newtonsoft.Json;
using VnvcStaffAdmin.Application.Helpers;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Dtos.Elasticsearchs;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Application.ExtendServices.ElasticSearch
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly IElasticClient _elasticClient;
        private readonly IndexManager _indexManager;
        private readonly IVnvcStaffUow _vnvcStaffUow;

        public ElasticsearchService(
            IElasticClient elasticClient, 
            IVnvcStaffUow vnvcStaffUow)
        {
            _elasticClient = elasticClient;
            _indexManager = new IndexManager(elasticClient);
            _vnvcStaffUow = vnvcStaffUow;
        }

        public async Task IndexDocumentAsync<T>(T document) where T : class
        {
            var indexName = GetIndexName<T>();

            await _indexManager.EnsureIndexExistsAsync<T>(indexName);

            var indexDescriptor = new IndexDescriptor<T>(document)
                .Index(indexName);

            var response = await _elasticClient.IndexAsync(indexDescriptor);
            if (!response.IsValid)
            {
                var errorMessage = $"Failed to index document: {response.OriginalException?.Message} | Response: {response.DebugInformation}";
                throw new Exception(errorMessage);
            }
        }

        public async Task<T> GetDocumentAsync<T>(string id) where T : class
        {
            var indexName = GetIndexName<T>();

            var response = await _elasticClient.GetAsync<T>(id, g => g.Index(indexName));
            if (!response.IsValid)
            {
                throw new Exception($"Failed to get document: {response.OriginalException?.Message} | Response: {response.DebugInformation}");
            }

            return response.Source;
        }

        public async Task DeleteDocumentAsync<T>(string id) where T : class
        {
            var indexName = GetIndexName<T>();
            var response = await _elasticClient.DeleteAsync<T>(id, g => g.Index(indexName));
            if (!response.IsValid)
            {
                throw new Exception($"Failed to delete document: {response.OriginalException.Message}");
            }
        }

        public async Task<IEnumerable<T>> SearchDocumentsAsync<T>(string query) where T : class
        {
            var searchResponse = await _elasticClient.SearchAsync<T>(s => s
                .Query(q => q
                    .QueryString(d => d
                        .Query(query)
                    )
                )
            );

            if (!searchResponse.IsValid)
            {
                throw new Exception($"Failed to search documents: {searchResponse.OriginalException.Message}");
            }

            return searchResponse.Documents;
        }

        public async Task<DatasourceResult<T>> SearchAsync<T>(ElasticSearchQuery query) where T : class
        {
            try
            {
                var indexName = GetIndexName<T>();

                var isNotDeleteQuery = new BoolQuery
                {
                    Should =
                    [
                        new BoolQuery { MustNot = [new ExistsQuery { Field = "isDelete" }] },
                    new TermQuery { Field = "isDelete", Value = false }
                    ],
                    MinimumShouldMatch = 1
                };

                var searchDescriptor = new SearchDescriptor<T>()
                    .Index(indexName)
                    .From(query.From)
                    .Size(query.Size);

                if (query.Query != null)
                {
                    searchDescriptor = searchDescriptor.Query(q => q
                        .Bool(b => b
                            .Must(
                                m => m.Raw(query.Query.ToString()),
                                m => isNotDeleteQuery
                            )
                        )
                    );
                }
                else
                {
                    searchDescriptor = searchDescriptor.Query(q => isNotDeleteQuery);
                }

                if (query.Sort != null)
                {
                    searchDescriptor = searchDescriptor.Sort(s => s
                        .Field(f => f.Field(query.Sort.Field)
                        .Order(query.Sort.Ascending ? SortOrder.Ascending : SortOrder.Descending)));
                }

                if (query.Source != null)
                {
                    searchDescriptor = searchDescriptor.Source(src => src
                        .Includes(i => i.Fields(query.Source.Includes?.ToArray() ?? Array.Empty<string>()))
                        .Excludes(e => e.Fields(query.Source.Excludes?.ToArray() ?? Array.Empty<string>())));
                }

                if (query.Indices != null && query.Indices.Any())
                {
                    searchDescriptor = searchDescriptor.Index(query.Indices.ToArray());
                }


                if (query.Aggregations != null && query.Aggregations.Any())
                {
                    searchDescriptor = searchDescriptor.Aggregations(a =>
                    {
                        foreach (var agg in query.Aggregations)
                        {
                            a.Terms(agg.Key, t => t.Field(agg.Value.ToString()));
                        }
                        return a;
                    });
                }

                var elasticResponse = await _elasticClient.SearchAsync<T>(searchDescriptor);

                return new DatasourceResult<T>
                {
                    From = query.From,
                    Size = query.Size,
                    Total = elasticResponse.Total,
                    Data = [.. elasticResponse.Documents]
                };

            }
            catch (Exception ex)
            {
                return new DatasourceResult<T>
                {
                    From = query.From,
                    Size = query.Size,
                    Total = 0,
                    Data = null
                };
            }
            

        }


        private string GetIndexName<T>() where T : class
        {
            Type? type = typeof(T);

            while (type != null)
            {
                var attribute = type.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                    .FirstOrDefault() as BsonCollectionAttribute;

                if (attribute != null)
                {
                    return attribute.CollectionName;
                }

                type = type.BaseType;
            }

            return typeof(T).Name.ToLowerInvariant();
        }
    }
}