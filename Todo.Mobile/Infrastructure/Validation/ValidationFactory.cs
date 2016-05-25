using Autofac;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Validation
{
    public class ValidationFactory : IValidationFactory
    {
        IComponentContext container;

        public ValidationFactory(IComponentContext container)
        {
            this.container = container;
        }

        public IEnumerable<IValidator<T>> GetValidators<T>()
        {
            var validators = container.Resolve<IEnumerable<IValidator<T>>>();
            return validators;
        }
    }
}
