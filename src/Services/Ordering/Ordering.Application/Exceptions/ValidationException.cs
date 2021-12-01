using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(string message = null,
            IEnumerable<ValidationFailure> validationFailures = null,
            Exception innerException = null)
            : base(message, innerException)
        {
            if (validationFailures is null)
                Errors = new Dictionary<string, string[]>();
            else
                Errors = validationFailures
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(g => g.Key, g => g.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; set; }
    }
}
