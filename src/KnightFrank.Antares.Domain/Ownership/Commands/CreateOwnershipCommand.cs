namespace KnightFrank.Antares.Domain.Ownership.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public class CreateOwnershipCommand : IRequest<Guid>
    {
        public CreateOwnershipCommand()
        {
            this.ContactIds = new List<Guid>();
        }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? SellDate { get; set; }

        public ICollection<Guid> ContactIds { get; set; }

        public Guid PropertyId { get; set; }

        public decimal? BuyPrice { get; set; }

        public decimal? SellPrice { get; set; }

        public Guid OwnershipTypeId { get; set; }
    }
}
