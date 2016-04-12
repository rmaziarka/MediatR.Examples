namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property;

    using MediatR;

    public class ActivityQuery : IRequest<Activity>
    {
        public Guid Id { get; set; }
    }
}
