using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config
{
    public interface IServiceLocator
    {
        T GetService<T>();
        object GetService(Type type);
    }
}
