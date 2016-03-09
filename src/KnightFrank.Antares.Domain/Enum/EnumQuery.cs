namespace KnightFrank.Antares.Domain.Enum
{
    using MediatR;
    public class EnumQuery : IRequest<EnumQueryResult>
    {
        public string Code { get; set; }
    }
}