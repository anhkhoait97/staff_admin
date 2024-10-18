using MongoDB.Bson.Serialization.Attributes;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Constants;

namespace VnvcStaffAdmin.Domain.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection(VnvcStaffCollection.AppAccount)]
    public class AppAccount : BaseSoftDeleteEntity
    {
        public string? FullName { get; set; }

        public string? UserName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Province { get; set; }

        public string? District { get; set; }

        public string? Ward { get; set; }

        public string? Address { get; set; }

        public string? AvatarUrl { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? Birthday { get; set; }

        public string? Gender { get; set; }

        public bool IsActive { get; set; }

        public List<DeviceAppAccountInfo> Devices { get; set; } = [];

        public List<LogLoginAppAccount> LoginLog { get; set; } = [];

        public string? Center { get; set; }

        public string? IdentityNumber { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class DeviceAppAccountInfo
    {
        public string? DeviceId { get; set; }

        public string? TokenFirebase { get; set; }

        public string? SignalRConnectionId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? At { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class LogLoginAppAccount
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? At { get; set; }
        public string? Ip { get; set; }
        public string? LogType { get; set; }
        public string? DeviceId { get; set; }
        public string? TokenFirebase { get; set; }
    }
}
