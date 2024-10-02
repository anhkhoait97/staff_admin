using System.ComponentModel.DataAnnotations;
using VnvcStaffAdmin.Domain.Interface;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Domain.Dtos.Recruitments
{
    public class UpdateRecruitmentDto : Recruitment, IUpdateColumn
    {
        [Required]
        public required List<string> Columns { get; set; }
    }
}