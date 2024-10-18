using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Dtos.Elasticsearchs;
using VnvcStaffAdmin.Domain.Dtos.New;
using VnvcStaffAdmin.Domain.Dtos.NewCategories;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Application.Services.Interfaces
{
    public interface INewsCategoryService
    {
        Task<DatasourceResult<NewsCategory>> GetByQuery(ElasticSearchQuery query);
        Task<ResponseModel> GetByIdAsync(string id);
        Task<DatasourceResult<NewsCategory>> GetLists(QueryGetListNewsCategoryDto query);
        Task<ResponseModel> AddAsync(CreateNewCategoryDto data);
        Task<ResponseModel> UpdateAsync(UpdateNewsCategoryDto entity);
    }
}
