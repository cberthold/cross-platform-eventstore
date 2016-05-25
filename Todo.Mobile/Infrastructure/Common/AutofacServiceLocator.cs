using Autofac;
using Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class AutofacServiceLocator : IServiceLocator
    {

        readonly ILifetimeScope container;

        public AutofacServiceLocator(ILifetimeScope container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        public object GetService(Type type)
        {
            return container.Resolve(type);
        }

        public T GetService<T>()
        {
            return container.Resolve<T>();
        }
    }
}
