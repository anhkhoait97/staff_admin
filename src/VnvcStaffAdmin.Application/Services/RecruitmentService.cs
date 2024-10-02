using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.Recruitments;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Application.Services
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly ILogger<RecruitmentService> _logger;
        private readonly IVnvcStaffUow _vnvcStaffUow;

        public RecruitmentService(
            ILoggerFactory loggerFactory,
            IVnvcStaffUow vnvcStaffUow)
        {
            _logger = loggerFactory.CreateLogger<RecruitmentService>();
            _vnvcStaffUow = vnvcStaffUow;
        }

        public async Task<ResponseModel<Recruitment>> GetByIdAsync(string id)
        {
            _vnvcStaffUow.GetCollection<Recruitment>().AsQueryable();

            var entity = await _vnvcStaffUow.GetRepository<Recruitment>().GetByIdAsync(id);
            return ResponseModel<Recruitment>.Successed(entity);
        }

        public async Task<ResponseModel> CreateAsync(CreateRecruitmentDto entity)
        {
            try
            {
                entity.CreatedAt = DateTime.Now;

                await _vnvcStaffUow.GetRepository<Recruitment>().AddAsync(entity);

                return ResponseModel.Successed("Thêm tin tức tuyển dụng thành công", entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return ResponseModel.Failed(ex.Message);
            }
        }

        public async Task<ResponseModel<Recruitment>> UpdateAsync(UpdateRecruitmentDto entity)
        {
            var response = new ResponseModel();
            try
            {
                var filter = Builders<Recruitment>.Filter.Eq("Id", entity.Id);

                var updateBuilder = Builders<Recruitment>.Update;

                var updateDefinition = updateBuilder.Combine(
                    entity.Columns.Select(column => column switch
                {
                    "Title" => updateBuilder.Set(s => s.Title, entity.Title),
                    "IsActive" => updateBuilder.Set(s => s.IsActive, entity.IsActive),
                    "AddressWork" => updateBuilder.Set(s => s.AddressWork, entity.AddressWork),
                    "NumberOfRecruits" => updateBuilder.Set(s => s.NumberOfRecruits, entity.NumberOfRecruits),
                    "Salary" => updateBuilder.Set(s => s.Salary, entity.Salary),
                    "Level" => updateBuilder.Set(s => s.Level, entity.Level),
                    "Experience" => updateBuilder.Set(s => s.Experience, entity.Experience),
                    "Degree" => updateBuilder.Set(s => s.Degree, entity.Degree),
                    "Deadline" => updateBuilder.Set(s => s.Deadline, entity.Deadline),
                    "SubmitDated" => updateBuilder.Set(s => s.SubmitDated, entity.SubmitDated),
                    "Description" => updateBuilder.Set(s => s.Description, entity.Description),
                    "IsDelete" => updateBuilder.Set(s => s.IsDelete, entity.IsDelete),
                    _ => null
                }).Where(update => update != null));

                var options = new FindOneAndUpdateOptions<Recruitment>
                {
                    ReturnDocument = ReturnDocument.After
                };

                var jobPost = await _vnvcStaffUow.GetRepository<Recruitment>().UpdateAsync(filter, updateDefinition, options);

                return ResponseModel<Recruitment>.Successed(jobPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel<Recruitment>.Failed(ex.Message);
            }
        }
    }
}