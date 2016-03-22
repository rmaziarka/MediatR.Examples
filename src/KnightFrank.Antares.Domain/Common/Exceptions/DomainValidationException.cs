namespace KnightFrank.Antares.Domain.Common.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DomainValidationException : Exception
    {
        public DomainValidationException()
        {
        }

        public DomainValidationException(string message)
            : base(message)
        {
        }

        public DomainValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DomainValidationException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}