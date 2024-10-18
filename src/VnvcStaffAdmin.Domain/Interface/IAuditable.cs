using MongoDB.Bson.Serialization.Attributes;

namespace VnvcStaffAdmin.Domain.Interface
{
    public interface IAuditable
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? UpdatedAt { get; set; }
    }
}