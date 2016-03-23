namespace KnightFrank.Antares.Domain.AddressForm.Queries
{
    using System;

    using KnightFrank.Antares.Domain.AddressForm.QueryResults;

    using MediatR;

    public class AddressFormByIdQuery : IRequest<AddressFormQueryResult>
    {
        public AddressFormByIdQuery(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; private set; }
    }
}
