using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Dtos.WorkSheets;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Application.Services.Interfaces
{
    public interface IWorkSheetService
    {
        Task<ResponseModel<WorkSheetHistoryDto>> GetDetailWorkSheetOfUser(QueryGetDetailWorkSheetHistoryDto query);

        Task<DatasourceResult<WorkSheetHistoryDto>> GetWorkSheets(QueryGetListWorkSheetDto query);

        Task<string> ExportWorkSheets(QueryGetListWorkSheetDto query);

        Task<string> ExportDetailWorkSheetOfUser(QueryGetDetailWorkSheetHistoryDto query);
    }
}
