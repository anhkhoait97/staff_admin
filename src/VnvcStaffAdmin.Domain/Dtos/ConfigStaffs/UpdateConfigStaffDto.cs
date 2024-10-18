using VnvcStaffAdmin.Domain.Interface;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Domain.Dtos.ConfigStaffs
{
    public class UpdateConfigStaffDto : ConfigStaff, IUpdateColumn
    {
        public List<string>? Columns { get; set; }
    }
}