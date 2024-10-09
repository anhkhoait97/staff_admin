using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Domain.Dtos.Banners
{
    public class CreatedBannerDto : Banner
    {
        public List<string> Columns { get; set; }
    }
}
