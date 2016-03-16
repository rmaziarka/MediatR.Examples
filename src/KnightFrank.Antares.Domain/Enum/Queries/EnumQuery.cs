namespace KnightFrank.Antares.Domain.Enum.Queries
{
    using MediatR;

    public class EnumQuery : IRequest<EnumQueryResult>
    {
        public string Code { get; set; }
    }
}
