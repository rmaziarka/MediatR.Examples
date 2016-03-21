namespace KnightFrank.Antares.Domain.Common.Exceptions
{
    using System;

    [Serializable]
    public class ResourceNotFoundException : Exception
    {
        public Guid Id { get; set; }

        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(string message, Guid id) : base(message)
        {
            this.Id = id;
        }
    }
}