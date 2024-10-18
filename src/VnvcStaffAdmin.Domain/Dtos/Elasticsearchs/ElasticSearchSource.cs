using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Domain.Dtos.Elasticsearchs
{
    public class ElasticSearchSource
    {
        public string[]? Includes { get; set; } = [];
        public string[]? Excludes { get; set; } = [];
    }
}
