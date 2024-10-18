using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Dtos.Elasticsearchs;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Application.ExtendServices.ElasticSearch
{
    public interface IElasticsearchService
    {
        Task IndexDocumentAsync<T>(T document) where T : class;
        Task<T> GetDocumentAsync<T>(string id) where T : class;
        Task DeleteDocumentAsync<T>(string id) where T : class;
        Task<IEnumerable<T>> SearchDocumentsAsync<T>(string query) where T : class;
        Task<DatasourceResult<T>> SearchAsync<T>(ElasticSearchQuery query) where T : class;
    }
}
