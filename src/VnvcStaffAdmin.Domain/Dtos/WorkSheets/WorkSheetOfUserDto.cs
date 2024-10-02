using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Domain.Dtos.WorkSheets
{
    public class WorkSheetOfUserDto
    {
        public string? Id { get; set; }

        public string? DayShift { get; set; }

        public int? CheckInHour { get; set; }

        public int? CheckInMinutes { get; set; }

        public int? CheckOutHour { get; set; }

        public int? CheckOutMinutes { get; set; }

        public string? NumberOfHoursWorked { get; set; }
    }
}
