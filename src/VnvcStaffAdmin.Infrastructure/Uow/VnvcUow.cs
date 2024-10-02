using VnvcStaffAdmin.Infrastructure.Interface.DbContext;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Infrastructure.Uow
{
    public class VnvcUow : UnitOfWork, IVnvcUow
    {
        public VnvcUow(IVnvcContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }
    }
}