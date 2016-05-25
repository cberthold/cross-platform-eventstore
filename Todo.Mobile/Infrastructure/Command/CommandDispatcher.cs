
using Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Exceptions;
using FluentValidation.Results;

namespace Infrastructure.Command
{
    public class CommandDispatcher
    {
        ICommandSender bus;
        IValidationFactory validationFactory;

        public CommandDispatcher(ICommandSender bus, IValidationFactory validationFactory)
        {
            this.bus = bus;
            this.validationFactory = validationFactory;
        }

        public void Send<T>(T command)
            where T : ICommand
        {
            // validate command - throws exception if not valid
            ValidateCommand(command);
            // send command to bus if it validates
            bus.Send(command);
        }

        private void ValidateCommand<T>(T command)
        {
            var results = new List<ValidationResult>();

            foreach (var validator in validationFactory.GetValidators<T>())
            {
                var result = validator.Validate(command);

                if (!result.IsValid)
                {
                    results.Add(result);
                }
            }

            if (results.Count > 0)
            {
                throw new ValidationException(results);
            }

        }
    }
}
