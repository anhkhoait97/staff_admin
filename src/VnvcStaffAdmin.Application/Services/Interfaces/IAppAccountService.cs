using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Dtos.AppAccounts;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Application.Services.Interfaces
{
    public interface IAppAccountService
    {
        Task<ResponseModel> Update(UpdateAppAccountDto dto);

        Task<string> Export(QueryGetListAppAccountDto query);

        Task<ResponseModel> GetById(string id);

        Task<DatasourceResult<AppAccount>> GetLists(QueryGetListAppAccountDto query);
    }
}
