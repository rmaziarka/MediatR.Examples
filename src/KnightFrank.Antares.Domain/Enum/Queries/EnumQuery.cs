namespace KnightFrank.Antares.Domain.Enum.Queries
{
    using KnightFrank.Antares.Domain.Enum.QueryResults;

    using MediatR;

    public class EnumQuery : IRequest<EnumQueryResult>
    {
        public string Code { get; set; }
    }
}
