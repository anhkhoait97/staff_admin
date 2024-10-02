using MongoDB.Bson.Serialization.Attributes;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Constants;

namespace VnvcStaffAdmin.Domain.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection(VnvcStaffCollection.TermAndCondition)]
    public class TermAndCondition : BaseEntity
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? Type { get; set; }
    }
}
