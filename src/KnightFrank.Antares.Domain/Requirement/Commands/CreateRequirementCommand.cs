namespace KnightFrank.Antares.Domain.Requirement.Commands
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Common.Commands;

    using MediatR;

    public class CreateRequirementCommand : IRequest<Guid>
    {
        public CreateRequirementCommand()
        {
            this.ContactIds = new List<Guid>();
        }

        public DateTime CreateDate { get; set; }

        public IList<Guid> ContactIds { get; set; }

        public CreateOrUpdateAddress Address { get; set; }

        public string Description { get; set; }

        public Guid RequirementTypeId { get; set; }

        public decimal? RentMin { get; set; }
        public decimal? RentMax { get; set; }
    }
}