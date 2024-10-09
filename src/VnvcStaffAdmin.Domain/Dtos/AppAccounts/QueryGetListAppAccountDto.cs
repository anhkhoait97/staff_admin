using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Interface;

namespace VnvcStaffAdmin.Domain.Dtos.AppAccounts
{
    public class QueryGetListAppAccountDto : IQueryPaging
    {
        public string? SearchText { get; set; }
        public int From { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
