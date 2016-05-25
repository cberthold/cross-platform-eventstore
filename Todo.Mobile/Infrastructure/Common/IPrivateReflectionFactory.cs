using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Mobile.Infrastructure.Common
{
    public interface IPrivateReflectionFactory
    {
        object CallMethod(object target, string methodName, params object[] parms);
        dynamic AsDynamic(object o);
    }
}
