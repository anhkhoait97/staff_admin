using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using VnvcStaffAdmin.Application.Extensions;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.New;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Application.Services
{
    public class NewsServices : INewsServices
    {
        private readonly ILogger<NewsServices> _logger;
        private readonly IVnvcStaffUow _vnvcStaffUow;

        public NewsServices(
            ILoggerFactory loggerFactory,
            IVnvcStaffUow vnvcStaffUow)
        {
            _logger = loggerFactory.CreateLogger<NewsServices>();
            _vnvcStaffUow = vnvcStaffUow;
        }

        public async Task<ResponseModel> GetById(string id)
        {
            try
            {
                var news = await _vnvcStaffUow.GetRepository<News>().SingleAsync(x => x.Id == id);

                if (news.NewsRelateds.Any())
                {
                    var newsRelateds = await _vnvcStaffUow.GetRepository<News>().FindAsync(x => news.NewsRelateds.Contains(x.Id));

                    news.DisplayNewsRelateds = newsRelateds.ToList();
                }

                return news != null ? ResponseModel.Successed("Thành công", news) : ResponseModel.Failed("Tài khoản không tồn tại");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Failed(ex.Message);
            }
        }

        public async Task<DatasourceResult<AppAccount>> GetLists(QueryGetListNewsDto query)
        {
            var result = new DatasourceResult<AppAccount>
            {
                From = query.From,
                Size = query.Size
            };

            try
            {
                var news = await _vnvcStaffUow.GetRepository<AppAccount>().FindPagingAsync(x => true, query.From, query.Size, Builders<AppAccount>.Sort.Descending(f => f.CreatedAt));
                result.Data = news.ToList();
                result.Total = await _vnvcStaffUow.GetRepository<AppAccount>().CountAsync(x => true);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<ResponseModel> Create(CreatedNewsDto data)
        {
            try
            {
                var news = data.Cast<News>();

                if (!string.IsNullOrEmpty(data.ViewAdvise))
                {
                    try
                    {
                        news.ViewAdviseData = JsonConvert.DeserializeObject<ViewAdviseDataDetail>(data.ViewAdvise);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                await _vnvcStaffUow.GetRepository<News>().AddAsync(news);

                return ResponseModel.Successed("Thành công", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Failed(ex.Message);
            }
        }

        public async Task<ResponseModel> Update(CreatedNewsDto dto)
        {
            try
            {
                var news = await _vnvcStaffUow.GetRepository<News>().SingleAsync(x => x.Id == dto.Id);

                if (news == null) return ResponseModel.Failed("Tin tức không tồn tại");

                var filter = Builders<News>.Filter.Eq("Id", dto.Id);

                var updateBuilder = Builders<News>.Update;

                var updateDefinition = updateBuilder.Combine(
                    dto.Columns.Select(column => column switch
                    {
                        "Title" => updateBuilder.Set(s => s.Title, dto.Title),
                        "Order" => updateBuilder.Set(s => s.Order, dto.Order),
                        "Description" => updateBuilder.Set(s => s.Description, dto.Description),
                        "Content" => updateBuilder.Set(s => s.Content, dto.Content),
                        "Type" => updateBuilder.Set(s => s.Type, dto.Type),
                        "IsHotNews" => updateBuilder.Set(s => s.IsHotNews, dto.IsHotNews),
                        "IsActive" => updateBuilder.Set(s => s.IsActive, dto.IsActive),
                        "NewsCategoryIds" => updateBuilder.Set(s => s.NewsCategoryIds, dto.NewsCategoryIds),
                        "Group" => updateBuilder.Set(s => s.Group, dto.Group),
                        "ObjectApply" => updateBuilder.Set(s => s.ObjectApply, dto.ObjectApply),
                        "NewsCategoryJoinIds" => updateBuilder.Set(s => s.NewsCategoryJoinIds, dto.NewsCategoryJoinIds),
                        "ObjectId" => updateBuilder.Set(s => s.ObjectId, dto.ObjectId),
                        "Source" => updateBuilder.Set(s => s.Source, dto.Source),
                        "VideoLink" => updateBuilder.Set(s => s.VideoLink, dto.VideoLink),
                        "IsSendNoti" => updateBuilder.Set(s => s.IsSendNoti, dto.IsSendNoti),
                        "UserSendNotis" => updateBuilder.Set(s => s.UserSendNotis, dto.UserSendNotis),
                        "NewsRelateds" => updateBuilder.Set(s => s.NewsRelateds, dto.NewsRelateds),
                        _ => null
                    }).Where(update => update != null));

                var options = new FindOneAndUpdateOptions<News>
                {
                    ReturnDocument = ReturnDocument.After
                };

                var data = await _vnvcStaffUow.GetRepository<News>().UpdateAsync(filter, updateDefinition, options);

                return ResponseModel.Successed("Cập nhật thành công", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Failed(ex.Message);
            }
        }
    }
}
