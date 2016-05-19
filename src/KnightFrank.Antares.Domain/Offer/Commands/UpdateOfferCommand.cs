namespace KnightFrank.Antares.Domain.Offer.Commands
{
    using System;

    using MediatR;

    public class UpdateOfferCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public Guid StatusId { get; set; }

        public decimal Price { get; set; }

        public DateTime OfferDate { get; set; }

        public DateTime? ExchangeDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public string SpecialConditions { get; set; }
    }
}
