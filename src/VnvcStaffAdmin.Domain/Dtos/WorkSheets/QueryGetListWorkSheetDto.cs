using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Domain.Dtos.WorkSheets
{
    public class QueryGetListWorkSheetDto
    {
        public int From { get; set; } = 0;

        public int Size { get; set; } = 10;

        public int Month { get; set; }

        public int Year { get; set; }
    }
}
