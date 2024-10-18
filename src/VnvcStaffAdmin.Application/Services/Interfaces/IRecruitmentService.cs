using VnvcStaffAdmin.Domain.Dtos.New;
using VnvcStaffAdmin.Domain.Dtos.Recruitments;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Application.Services.Interfaces
{
    public interface IRecruitmentService
    {
        Task<ResponseModel<Recruitment>> GetByIdAsync(string id);
        Task<DatasourceResult<Recruitment>> GetLists(QueryGetListRecruitmentDto query);
        Task<ResponseModel> CreateAsync(CreateRecruitmentDto entity);

        Task<ResponseModel<Recruitment>> UpdateAsync(UpdateRecruitmentDto entity);
    }
}