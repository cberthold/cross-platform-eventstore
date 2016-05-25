using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Mobile.Infrastructure.Common;
using Xamarin.Forms;

namespace Infrastructure.Common
{
    internal static class PrivateReflectionDynamicObjectExtensions
    {
        
        public static dynamic AsDynamic(this object o)
        {
            var factory = DependencyService.Get<IPrivateReflectionFactory>();
            
            return factory.AsDynamic(o);
        }
    }
}
