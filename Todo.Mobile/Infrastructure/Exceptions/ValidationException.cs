using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public class ValidationException : Exception
    {
        [DataMember]
        public IEnumerable<ValidationResult> Errors { get; protected set; }

        public ValidationException(IEnumerable<ValidationResult> errors) : base()
        {
            this.Errors = errors;
        }
    }
}
