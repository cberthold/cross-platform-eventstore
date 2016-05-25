using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messages
{
    public interface IHandler<in T> where T : IMessage
    {
        void Handle(T message);
    }
}
