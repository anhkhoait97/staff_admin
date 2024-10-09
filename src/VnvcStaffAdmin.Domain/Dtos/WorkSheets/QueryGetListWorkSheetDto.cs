using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Interface;

namespace VnvcStaffAdmin.Domain.Dtos.WorkSheets
{
    public class QueryGetListWorkSheetDto : IQueryPaging
    {
        public int Month { get; set; }

        public int Year { get; set; }
        public int From { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
