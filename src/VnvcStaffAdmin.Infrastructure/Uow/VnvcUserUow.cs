using VnvcStaffAdmin.Infrastructure.Interface.DbContext;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Infrastructure.Uow
{
    public class VnvcUserUow : UnitOfWork, IVnvcUserUow
    {
        public VnvcUserUow(IVnvcUserContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }
    }
}