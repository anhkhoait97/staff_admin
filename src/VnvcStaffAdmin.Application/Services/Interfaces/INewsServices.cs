using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Dtos.New;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Application.Services.Interfaces
{
    public interface INewsServices
    {
        Task<ResponseModel> GetById(string id);

        Task<DatasourceResult<AppAccount>> GetLists(QueryGetListNewsDto query);

        Task<ResponseModel> Create(CreatedNewsDto data);

        Task<ResponseModel> Update(CreatedNewsDto dto);
    }
}
