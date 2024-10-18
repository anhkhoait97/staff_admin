using MongoDB.Bson.Serialization.Attributes;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Constants;

namespace VnvcStaffAdmin.Domain.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection(VnvcStaffCollection.WorkSheet)]
    public class WorkSheet : BaseSoftDeleteEntity
    {
        public string? DayShift { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? DayShiftDate { get; set; }

        public string? UserId { get; set; }

        public int? CheckInHour { get; set; }

        public int? CheckInMinutes { get; set; }

        public int? CheckOutHour { get; set; }

        public int? CheckOutMinutes { get; set; }

        public double? TotalWorkingHour { get; set; }

        public List<InfoDetail>? SSID { get; set; } = [];

        public string? Address { get; set; }

        public List<InfoDetail>? GPS { get; set; } = [];

        public List<LogDetail>? Logs { get; set; } = [];
    }

    public class InfoDetail
    {
        public string? Data { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? CreatedAt { get; set; }
    }

    public class LogDetail
    {
        public string? SSID { get; set; }

        public string? GPS { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? CreatedAt { get; set; }
    }
}
