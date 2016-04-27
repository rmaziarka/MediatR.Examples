namespace KnightFrank.Antares.Domain.Enum.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Enum.QueryResults;

    using MediatR;

    public class EnumItemQuery : IRequest<Dictionary<string, ICollection<EnumItemResult>>>
    {
    }
}
