using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.TermAndConditions;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Application.Services
{
    public class TermAndConditionService : ITermAndConditionService
    {
        private readonly ILogger<TermAndConditionService> _logger;
        private readonly IVnvcStaffUow _vnvcStaffUow;

        public TermAndConditionService (
            ILoggerFactory loggerFactory,
            IVnvcStaffUow vnvcStaffUow)
        {
            _logger = loggerFactory.CreateLogger<TermAndConditionService>();
            _vnvcStaffUow = vnvcStaffUow;
        }

        public async Task<ResponseModel> GetByType(string type)
        {
            try
            {
                var result = await _vnvcStaffUow.GetRepository<TermAndCondition>().SingleAsync(x => x.Type == type);

                return ResponseModel.Successed("Thành công", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Failed(ex.Message);
            }
        }

        public async Task<ResponseModel> CreateOrUpdate(CreateTermAndConditionDto dto)
        {
            try
            {
                var term = await _vnvcStaffUow.GetRepository<TermAndCondition>().SingleAsync(x => x.Type == dto.Type);
                if (term == null)
                {
                    await _vnvcStaffUow.GetRepository<TermAndCondition>().AddAsync(dto);

                    return ResponseModel.Successed("Thêm thành công", dto);
                }
                else
                {
                    var filter = Builders<TermAndCondition>.Filter.Eq("Id", dto.Id);

                    var update = Builders<TermAndCondition>.Update
                       .Set(s => s.Content, dto.Content)
                       .Set(s => s.Title, dto.Title);

                    var options = new FindOneAndUpdateOptions<TermAndCondition>
                    {
                        ReturnDocument = ReturnDocument.After
                    };

                    var data = await _vnvcStaffUow.GetRepository<TermAndCondition>().UpdateAsync(filter, update, options);

                    return ResponseModel.Successed("Cập nhật thành công", data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Failed(ex.Message);
            }
        }
    }
}
