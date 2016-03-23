namespace KnightFrank.Antares.Domain.Common.Exceptions
{
    using System;

    [Serializable]
    public class ResourceNotFoundException : Exception
    {
        public Guid ResourceId { get; set; }

        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(string message, Guid resourceId) : base(message)
        {
            this.ResourceId = resourceId;
        }
    }
}