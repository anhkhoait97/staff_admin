using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Domain.Dtos.New
{
    public class CreatedNewsDto : News
    {
        public string ViewAdvise { get; set; }
        public string ViewVaccine { get; set; }

        public List<string> Columns { get; set; }
    }
}
