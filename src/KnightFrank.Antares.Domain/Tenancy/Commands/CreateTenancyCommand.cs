namespace KnightFrank.Antares.Domain.Tenancy.Commands
{
    using System;

    using MediatR;

    public class CreateTenancyCommand : IRequest<Guid>
    {
    }
}