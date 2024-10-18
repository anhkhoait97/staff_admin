using VnvcStaffAdmin.Domain.Dtos.ConfigStaffs;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Application.Services.Interfaces
{
    public interface IConfigStaffService
    {
        Task<ResponseModel> GetByIdAsync(string id);

        Task<DatasourceResult<ConfigStaff>> GetLists(QueryGetListConfigStaffDto query);

        Task<ResponseModel> AddAsync(CreateConfigStaffDto data);

        Task<ResponseModel> UpdateAsync(UpdateConfigStaffDto entity);
    }
}