using System;
using VnvcStaffAdmin.Domain.Dtos.TermAndConditions;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Application.Services.Interfaces
{
    public interface ITermAndConditionService
    {
        Task<ResponseModel> GetByType(string type);

        Task<ResponseModel> CreateOrUpdate(CreateTermAndConditionDto dto);
    }
}
