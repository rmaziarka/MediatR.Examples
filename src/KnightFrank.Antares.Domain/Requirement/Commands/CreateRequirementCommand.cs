namespace KnightFrank.Antares.Domain.Requirement.Command
{

    using MediatR;
    using System;
    public class CreateRequirementCommand : IRequest<int>
    {
        public DateTime CreateDate { get; set; }
    }
}