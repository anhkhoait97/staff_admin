using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Domain.Dtos.WorkSheets
{
    public class WorkSheetHistoryDto
    {
        public string? UserId { get; set; }

        public string? FullName { get; set; }

        public List<WorkSheetOfUserDto> WorkSheets { get; set; } = new List<WorkSheetOfUserDto>();
    }
}
