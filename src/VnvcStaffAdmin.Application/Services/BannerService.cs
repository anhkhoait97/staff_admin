using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Application.Extensions;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.Banners;
using VnvcStaffAdmin.Domain.Dtos.New;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Application.Services
{
    public class BannerService : IBannerService
    {
        private readonly ILogger<BannerService> _logger;
        private readonly IVnvcStaffUow _vnvcStaffUow;

        public BannerService(
            ILoggerFactory loggerFactory,
            IVnvcStaffUow vnvcStaffUow)
        {
            _logger = loggerFactory.CreateLogger<BannerService>();
            _vnvcStaffUow = vnvcStaffUow;
        }

        public async Task<ResponseModel> GetById(string id)
        {
            try
            {
                var banners = await _vnvcStaffUow.GetRepository<Banner>().SingleAsync(x => x.Id == id);

                return ResponseModel.Successed("Thành công", banners);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Failed(ex.Message);
            }
        }

        public async Task<DatasourceResult<Banner>> GetLists(QueryGetListBannerDto query)
        {
            var result = new DatasourceResult<Banner>
            {
                From = query.From,
                Size = query.Size
            };

            try
            {
                var banners = await _vnvcStaffUow.GetRepository<Banner>().FindPagingAsync(x => true, query.From, query.Size, Builders<Banner>.Sort.Descending(f => f.CreatedAt));
                result.Data = banners.ToList();
                result.Total = await _vnvcStaffUow.GetRepository<Banner>().CountAsync(x => true);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<ResponseModel> Create(CreatedBannerDto data)
        {
            try
            {
                var banner = data.Cast<Banner>();

                await _vnvcStaffUow.GetRepository<Banner>().AddAsync(banner);

                return ResponseModel.Successed("Thành công", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Failed(ex.Message);
            }
        }

        public async Task<ResponseModel> Update(CreatedBannerDto dto)
        {
            try
            {
                var banner = await _vnvcStaffUow.GetRepository<Banner>().SingleAsync(x => x.Id == dto.Id);

                if (banner == null) return ResponseModel.Failed("Banner không tồn tại");

                var filter = Builders<Banner>.Filter.Eq("Id", dto.Id);

                var updateBuilder = Builders<Banner>.Update;

                var updateDefinition = updateBuilder.Combine(
                    dto.Columns.Select(column => column switch
                    {
                        "Name" => updateBuilder.Set(s => s.Name, dto.Name),
                        "Order" => updateBuilder.Set(s => s.Order, dto.Order),
                        "Description" => updateBuilder.Set(s => s.Description, dto.Description),
                        "Link" => updateBuilder.Set(s => s.Link, dto.Link),
                        "ImageUrl" => updateBuilder.Set(s => s.ImageUrl, dto.ImageUrl),
                        "Type" => updateBuilder.Set(s => s.Type, dto.Type),
                        "IsActive" => updateBuilder.Set(s => s.IsActive, dto.IsActive),
                        "StartDate" => updateBuilder.Set(s => s.StartDate, dto.StartDate),
                        "EndDate" => updateBuilder.Set(s => s.EndDate, dto.EndDate),
                        "Ratio" => updateBuilder.Set(s => s.Ratio, dto.Ratio),
                        "VideoLink" => updateBuilder.Set(s => s.VideoLink, dto.VideoLink),
                        "Config" => updateBuilder.Set(s => s.Config, dto.Config),
                        _ => null
                    }).Where(update => update != null));

                var options = new FindOneAndUpdateOptions<Banner>
                {
                    ReturnDocument = ReturnDocument.After
                };

                var data = await _vnvcStaffUow.GetRepository<Banner>().UpdateAsync(filter, updateDefinition, options);

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
