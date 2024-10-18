using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using VnvcStaffAdmin.Application.ExtendServices.ElasticSearch;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.ConfigStaffs;
using VnvcStaffAdmin.Domain.Dtos.Elasticsearchs;
using VnvcStaffAdmin.Domain.Dtos.NewCategories;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;
using VnvcStaffAdmin.Infrastructure.Uow;

namespace VnvcStaffAdmin.Application.Services
{
    public class ConfigStaffService : IConfigStaffService
    {
        private readonly ILogger<ConfigStaffService> _logger;
        private readonly IVnvcStaffUow _vnvcStaffUow;
        private readonly IElasticsearchService _elasticService;
        public ConfigStaffService(
            ILogger<ConfigStaffService> logger,
            IVnvcStaffUow vnvcStaffUow,
            IElasticsearchService elasticService)
        {
            _logger = logger;
            _vnvcStaffUow = vnvcStaffUow;
            _elasticService = elasticService;
        }
        public async Task<DatasourceResult<ConfigStaff>> GetByQuery(ElasticSearchQuery query)
        {
            return await _elasticService.SearchAsync<ConfigStaff>(query);
        }

        public async Task<ResponseModel> GetByIdAsync(string id)
        {
            var newsCateogry = await _elasticService.GetDocumentAsync<ConfigStaff>(id);
            return ResponseModel.Successed("Lấy thông tin chủ đề tin tức thành công", newsCateogry);
        }

        public async Task<ResponseModel> AddAsync(CreateConfigStaffDto data)
        {
            try
            {
                await _vnvcStaffUow.GetRepository<ConfigStaff>().AddAsync(data);
                await _elasticService.IndexDocumentAsync(data);
                return ResponseModel.Successed("Tạo chủ đề tin tức thành công", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Exception(ex.Message);
            }
        }

        public async Task<ResponseModel> UpdateAsync(UpdateConfigStaffDto entity)
        {
            try
            {
                var filter = Builders<ConfigStaff>.Filter.Eq("Id", entity.Id);

                var updateBuilder = Builders<ConfigStaff>.Update;

                var updateDefinition = updateBuilder.Combine(
                    entity.Columns?.Select(column => column switch
                    {
                        "LunchBreakHourFrom" => updateBuilder.Set(s => s.LunchBreakHourFrom, entity.LunchBreakHourFrom),
                        "LunchBreakHourTo" => updateBuilder.Set(s => s.LunchBreakHourTo, entity.LunchBreakHourTo),
                        "WorkingHourTo" => updateBuilder.Set(s => s.WorkingHourTo, entity.WorkingHourTo),
                        "NumberOfHoursWorked" => updateBuilder.Set(s => s.NumberOfHoursWorked, entity.NumberOfHoursWorked),
                        "WorkingHourFrom" => updateBuilder.Set(s => s.WorkingHourFrom, entity.WorkingHourFrom),
                        "EmailReceiveSubmitCV" => updateBuilder.Set(s => s.EmailReceiveSubmitCV, entity.EmailReceiveSubmitCV),
                        "SSID" => updateBuilder.Set(s => s.SSID, entity.SSID),
                        "IsDelete" => updateBuilder.Set(s => s.IsDelete, entity.IsDelete),
                        _ => null
                    }).Where(update => update != null));

                var options = new FindOneAndUpdateOptions<ConfigStaff>
                {
                    ReturnDocument = ReturnDocument.After
                };

                var result = await _vnvcStaffUow.GetRepository<ConfigStaff>().UpdateAsync(filter, updateDefinition, options);

                await _elasticService.IndexDocumentAsync(result);

                return ResponseModel.Successed("Cập nhật chủ đề tin tức thành công", result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<DatasourceResult<ConfigStaff>> GetLists(QueryGetListConfigStaffDto query)
        {
            var result = new DatasourceResult<ConfigStaff>
            {
                From = query.From,
                Size = query.Size
            };

            try
            {
                var news = await _vnvcStaffUow.GetRepository<ConfigStaff>().FindPagingAsync(x => true, query.From, query.Size, Builders<ConfigStaff>.Sort.Descending(f => f.CreatedAt));
                result.Data = news.ToList();
                result.Total = await _vnvcStaffUow.GetRepository<ConfigStaff>().CountAsync(x => true);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
    }
}