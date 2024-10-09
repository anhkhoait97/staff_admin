using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Interface;

namespace VnvcStaffAdmin.Domain.Dtos.Banners
{
    public class QueryGetListBannerDto : IQueryPaging
    {
        public int From { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
