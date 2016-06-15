namespace KnightFrank.Antares.Search.Common.Queries
{
    using MediatR;

    public class BasePageableQuery<T> : IRequest<T>
    {
        public int Page { get; set; }

        public int Size { get; set; }
    }
}
