using MongoDB.Bson.Serialization.Attributes;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Constants;

namespace VnvcStaffAdmin.Domain.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection(VnvcStaffCollection.WorkSheet)]
    public class WorkSheet : BaseEntity
    {
        public string? DayShift { get; set; }

        public DateTime? DayShiftDate { get; set; }

        public string? UserId { get; set; }

        public int? CheckInHour { get; set; }

        public int? CheckInMinutes { get; set; }

        public int? CheckOutHour { get; set; }

        public int? CheckOutMinutes { get; set; }

        public double? TotalWorkingHour { get; set; }

        public List<InfoDetail> SSID { get; set; } = new List<InfoDetail>();

        public string? Address { get; set; }

        public List<InfoDetail> GPS { get; set; } = new List<InfoDetail>();

        public List<LogDetail> Logs { get; set; } = new List<LogDetail> { };
    }

    public class InfoDetail
    {
        public string? Data { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class LogDetail
    {
        public string? SSID { get; set; }

        public string? GPS { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
