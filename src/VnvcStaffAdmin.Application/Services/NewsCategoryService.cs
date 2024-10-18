using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using VnvcStaffAdmin.Application.ExtendServices.ElasticSearch;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.Elasticsearchs;
using VnvcStaffAdmin.Domain.Dtos.New;
using VnvcStaffAdmin.Domain.Dtos.NewCategories;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Application.Services
{
    public class NewsCategoryService : INewsCategoryService
    {
        private readonly ILogger<NewsCategoryService> _logger;
        private readonly IVnvcStaffUow _vnvcStaffUow;
        private readonly IElasticsearchService _elasticService;

        public NewsCategoryService(
            ILogger<NewsCategoryService> logger,
            IVnvcStaffUow vnvcStaffUow,
            IElasticsearchService elasticService)
        {
            _logger = logger;
            _vnvcStaffUow = vnvcStaffUow;
            _elasticService = elasticService;
        }

        public async Task<DatasourceResult<NewsCategory>> GetByQuery(ElasticSearchQuery query)
        {
            return await _elasticService.SearchAsync<NewsCategory>(query);
        }

        public async Task<ResponseModel> GetByIdAsync(string id)
        {
            var newsCateogry = await _elasticService.GetDocumentAsync<NewsCategory>(id);
            return ResponseModel.Successed("Lấy thông tin chủ đề tin tức thành công", newsCateogry);
        }

        public async Task<ResponseModel> AddAsync(CreateNewCategoryDto data)
        {
            try
            {
                await _vnvcStaffUow.GetRepository<NewsCategory>().AddAsync(data);
                await _elasticService.IndexDocumentAsync(data);
                return ResponseModel.Successed("Tạo chủ đề tin tức thành công", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Exception(ex.Message);
            }
        }

        public async Task<ResponseModel> UpdateAsync(UpdateNewsCategoryDto entity)
        {
            try
            {
                var filter = Builders<NewsCategory>.Filter.Eq("Id", entity.Id);

                var updateBuilder = Builders<NewsCategory>.Update;

                var updateDefinition = updateBuilder.Combine(
                    entity.Columns.Select(column => column switch
                    {
                        "Title" => updateBuilder.Set(s => s.Title, entity.Title),
                        "Avatar" => updateBuilder.Set(s => s.Avatar, entity.Avatar),
                        "IsDelete" => updateBuilder.Set(s => s.IsDelete, entity.IsDelete),
                        _ => null
                    }).Where(update => update != null));

                var options = new FindOneAndUpdateOptions<NewsCategory>
                {
                    ReturnDocument = ReturnDocument.After
                };

                var result = await _vnvcStaffUow.GetRepository<NewsCategory>().UpdateAsync(filter, updateDefinition, options);

                await _elasticService.IndexDocumentAsync(result);

                return ResponseModel.Successed("Cập nhật chủ đề tin tức thành công" ,result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<DatasourceResult<NewsCategory>> GetLists(QueryGetListNewsCategoryDto query)
        {
            var result = new DatasourceResult<NewsCategory>
            {
                From = query.From,
                Size = query.Size
            };

            try
            {
                var news = await _vnvcStaffUow.GetRepository<NewsCategory>().FindPagingAsync(x => true, query.From, query.Size, Builders<NewsCategory>.Sort.Descending(f => f.CreatedAt));
                result.Data = news.ToList();
                result.Total = await _vnvcStaffUow.GetRepository<NewsCategory>().CountAsync(x => true);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
    }
}