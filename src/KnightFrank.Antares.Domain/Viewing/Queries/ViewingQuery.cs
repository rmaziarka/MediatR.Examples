namespace KnightFrank.Antares.Domain.Viewing.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property;

    using MediatR;

    public class ViewingQuery : IRequest<Viewing>
    {
        public Guid Id { get; set; }
    }
}
