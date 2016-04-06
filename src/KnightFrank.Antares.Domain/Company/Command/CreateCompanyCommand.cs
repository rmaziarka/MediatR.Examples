namespace KnightFrank.Antares.Domain.Company.Command
{
    using System;
    using System.Collections.Generic;

    using MediatR;
    public class CreateCompanyCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public List<Guid> ContactIds { get; set; }
    }
}