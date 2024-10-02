using VnvcStaffAdmin.Infrastructure.Interface.DbContext;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Infrastructure.Uow
{
    public class VnvcStaffUow : UnitOfWork, IVnvcStaffUow
    {
        public VnvcStaffUow(IVnvcStaffContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }
    }
}