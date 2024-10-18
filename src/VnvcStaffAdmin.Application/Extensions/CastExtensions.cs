using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Application.Extensions
{
    public static class CastExtensions
    {
        public static T Cast<T>(this Object myobj, List<string>? ignorFields = null)
        {
            var objectType = myobj.GetType();
            var target = typeof(T);
            var instance = Activator.CreateInstance(target, false);

            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var members = d.Where(memberInfo => d.Select(c => c.Name)
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo? propertyInfo;
            object? value;
            foreach (var memberInfo in members)
            {
                if (ignorFields != null && ignorFields.Any(n => n.ToLower() == memberInfo.Name.ToLower()))
                {
                    continue;
                }
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                value = myobj.GetType().GetProperty(memberInfo.Name)?.GetValue(myobj, null);

                propertyInfo?.SetValue(instance, value, null);
            }
            return (T)instance;
        }
    }
}
