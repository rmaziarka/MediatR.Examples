namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property.Activities;

    using MediatR;

    public class ActivityUserQuery : IRequest<ActivityUser>
    {
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }
    }
}
