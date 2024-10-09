using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Dtos.Banners;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Application.Services.Interfaces
{
    public interface IBannerService
    {
        Task<ResponseModel> GetById(string id);

        Task<DatasourceResult<Banner>> GetLists(QueryGetListBannerDto query);

        Task<ResponseModel> Create(CreatedBannerDto data);

        Task<ResponseModel> Update(CreatedBannerDto dto);
    }
}
