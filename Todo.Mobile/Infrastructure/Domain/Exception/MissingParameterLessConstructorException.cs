using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain.Exception
{
    public class MissingParameterLessConstructorException : System.Exception
    {
        public MissingParameterLessConstructorException(Type type)
            : base($"{type.FullName} has no constructor without paramerters. This can be either public or private")
        {
        }
    }
}
