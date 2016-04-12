namespace KnightFrank.Antares.Domain.Common.Exceptions
{
    using System;
    using System.Collections.Generic;

    using FluentValidation;
    using FluentValidation.Results;

    [Serializable]
    public class DomainValidationException : ValidationException
    {
        public DomainValidationException(IEnumerable<ValidationFailure> errors)
            : base("Domain validation exception occured", errors)
        {
        }

        public DomainValidationException(string propertyName, string error)
            : base(new[] { new ValidationFailure(propertyName, error) })
        {
        }

        public DomainValidationException(string propertyName) 
            : this(propertyName, "Value is incorrect")
        {
        }
    }
}