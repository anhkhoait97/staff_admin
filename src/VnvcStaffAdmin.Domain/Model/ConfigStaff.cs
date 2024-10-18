using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Constants;

namespace VnvcStaffAdmin.Domain.Model
{
    [BsonCollection(VnvcStaffCollection.ConfigStaff)]
    [BsonIgnoreExtraElements]
    public class ConfigStaff : BaseSoftDeleteEntity
    {
        public List<string> EmailReceiveSubmitCV { get; set; } = [];

        public List<SSIDInfo> SSID { get; set; } = [];

        public int LunchBreakHourFrom { get; set; }

        public int LunchBreakHourTo { get; set; }

        public int WorkingHourFrom { get; set; }

        public int WorkingHourTo { get; set; }

        public int NumberOfHoursWorked { get; set; }
    }
    public class SSIDInfo
    {
        public string? SSID { get; set; }

        public string? Address { get; set; }

        public string? BSSID { get; set; }

        public bool IsConnectionExpensive { get; set; }

        public string? Subnet { get; set; }

        public string? IpAddress { get; set; }
    }
}
