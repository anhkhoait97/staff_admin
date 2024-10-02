using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VnvcStaffAdmin.Domain.Interface;

namespace VnvcStaffAdmin.Domain.Model
{
    public abstract class BaseSoftDeleteEntity : IEntity, IAuditable, ISoftDeletable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public bool IsDelete { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public abstract class BaseEntity : IEntity, IAuditable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? CreatedBy { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? UpdatedAt { get; set; }
    }
}