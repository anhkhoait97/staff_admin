using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Domain.SettingModel
{
    public class MinioSettings
    {
        public string Endpoint { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public bool Secure { get; set; }
    }
}
