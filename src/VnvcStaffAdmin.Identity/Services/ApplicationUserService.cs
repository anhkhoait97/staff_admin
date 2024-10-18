using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Linq.Expressions;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Identity.Dtos.ApplicationUsers;
using VnvcStaffAdmin.Identity.Models;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Identity.Services
{
    public interface IApplicationUserService
    {
        Task<ResponseModel> Update(UpdateApplicationUserDto dto);

        Task<ResponseModel> GetById(string id);

        Task<DatasourceResult<ApplicationUser>> GetLists(QueryGetListApplicationUserDto query);
    }

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ILogger<ApplicationUserService> _logger;
        private readonly IVnvcUserUow _vnvcUserUow;

        public ApplicationUserService(
            ILoggerFactory loggerFactory,
            IVnvcUserUow vnvcUserUow)
        {
            _logger = loggerFactory.CreateLogger<ApplicationUserService>();
            _vnvcUserUow = vnvcUserUow;
        }

        public async Task<ResponseModel> GetById(string id)
        {
            try
            {
                var result = await _vnvcUserUow.GetRepository<ApplicationUser>().GetByIdAsync(id);

                return result != null ? ResponseModel.Successed("Thành công", result) : ResponseModel.Failed("Tài khoản không tồn tại");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Failed(ex.Message);
            }
        }

        public async Task<DatasourceResult<ApplicationUser>> GetLists(QueryGetListApplicationUserDto query)
        {
            var result = new DatasourceResult<ApplicationUser>
            {
                From = query.From,
                Size = query.Size
            };

            try
            {
                Expression<Func<ApplicationUser, bool>> predicate = x => true;

                if (!string.IsNullOrEmpty(query.SearchText))
                {
                    predicate = x => x.FullName.Contains(query.SearchText.Trim()) || x.Phone.Contains(query.SearchText.Trim()) || x.Email.Contains(query.SearchText.Trim());
                }

                var ApplicationUsers = await _vnvcUserUow.GetRepository<ApplicationUser>().FindPagingAsync(predicate, query.From, query.Size, Builders<ApplicationUser>.Sort.Descending(f => f.CreatedAt));
                result.Data = ApplicationUsers.ToList();
                result.Total = await _vnvcUserUow.GetRepository<ApplicationUser>().CountAsync(predicate);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<ResponseModel> Update(UpdateApplicationUserDto dto)
        {
            try
            {
                var account = await _vnvcUserUow.GetRepository<ApplicationUser>().SingleAsync(x => x.Id == dto.Id);

                if (account == null) return ResponseModel.Failed("Tài khoản không tồn tại");

                var filter = Builders<ApplicationUser>.Filter.Eq("Id", dto.Id);

                var updateBuilder = Builders<ApplicationUser>.Update;

                var updateDefinition = updateBuilder.Combine(
                    dto.Columns.Select(column => column switch
                    {
                        "FullName" => updateBuilder.Set(s => s.FullName, dto.FullName),
                        "Email" => updateBuilder.Set(s => s.Email, dto.Email),
                        "AvatarUrl" => updateBuilder.Set(s => s.AvatarUrl, dto.AvatarUrl),
                        "Birthday" => updateBuilder.Set(s => s.Birthday, dto.Birthday),
                        "Gender" => updateBuilder.Set(s => s.Gender, dto.Gender),
                        "IsActive" => updateBuilder.Set(s => s.IsActive, dto.IsActive),
                        "IsDelete" => updateBuilder.Set(s => s.IsDelete, dto.IsDelete),
                        _ => null
                    }).Where(update => update != null));
                
                var options = new FindOneAndUpdateOptions<ApplicationUser>
                {
                    ReturnDocument = ReturnDocument.After
                };

                var data = await _vnvcUserUow.GetRepository<ApplicationUser>().UpdateAsync(filter, updateDefinition, options);

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