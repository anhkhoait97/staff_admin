using MongoDB.Bson.Serialization.Attributes;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Constants;

namespace VnvcStaffAdmin.Domain.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection(VnvcStaffCollection.Recruitment)]
    public class Recruitment : BaseSoftDeleteEntity
    {
        public string? Title { get; set; }

        public bool IsActive { get; set; }

        //địa chỉ làm việc
        public string? AddressWork { get; set; }

        //số lượng tuyển
        public int NumberOfRecruits { get; set; }

        public string? Salary { get; set; }

        //cấp bậc
        public string? Level { get; set; }

        //kinh nghiệp
        public string? Experience { get; set; }

        //bằng cấp
        public string? Degree { get; set; }

        //hạn chót
        public DateTime Deadline { get; set; }

        //ngày đăng
        public DateTime SubmitDated { get; set; }

        //thông tin công việc
        public string? Description { get; set; }
    }
}