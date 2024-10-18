using VnvcStaffAdmin.Domain.Interface;
using VnvcStaffAdmin.Identity.Models;

namespace VnvcStaffAdmin.Identity.Dtos.ApplicationUsers
{
    public class UpdateApplicationUserDto : ApplicationUser, IUpdateColumn
    {
        public string? Id { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Province { get; set; }

        public string? District { get; set; }

        public string? Ward { get; set; }

        public string? Address { get; set; }

        public string? AvatarUrl { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Gender { get; set; }

        public bool IsActive { get; set; }

        public string? Center { get; set; }

        public string? IdentityNumber { get; set; }

        public List<string> Columns { get; set; } = new List<string>();
    }
}
