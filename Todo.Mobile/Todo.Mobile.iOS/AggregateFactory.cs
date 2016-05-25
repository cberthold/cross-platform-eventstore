using Infrastructure.Domain.Exception;
using Infrastructure.Domain.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(Todo.Mobile.iOS.AggregateFactory))]
namespace Todo.Mobile.iOS
{
    public class AggregateFactory : IAggregateFactory
    {
        public T CreateAggregate<T>()
        {
            try
            {
                return (T)Activator.CreateInstance(typeof(T), true);
            }
            catch (System.Exception ex)
            {
                throw new MissingParameterLessConstructorException(typeof(T));
            }
        }
    }
}
