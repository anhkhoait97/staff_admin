using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Infrastructure.Interface.DbContext;
using VnvcStaffAdmin.Infrastructure.Repositories.Interface;

namespace VnvcStaffAdmin.Infrastructure.Repositories
{
    public class RecruitmentRepository : BaseRepository<Recruitment>, IRecruitmentRepository
    {
        public RecruitmentRepository(IVnvcStaffContext context) : base(context)
        {
        }
    }
}