using VnvcStaffAdmin.Domain.Dtos.Recruitments;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Application.Services.Interfaces
{
    public interface IRecruitmentService
    {
        Task<ResponseModel<Recruitment>> GetByIdAsync(string id);

        Task<ResponseModel> CreateAsync(CreateRecruitmentDto entity);

        Task<ResponseModel<Recruitment>> UpdateAsync(UpdateRecruitmentDto entity);
    }
}