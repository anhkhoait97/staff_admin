using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Domain.Dtos.Elasticsearchs
{
    public class ElasticSearchSort
    {
        public string? Field { get; set; }
        public bool Ascending { get; set; } = true;
    }
}
