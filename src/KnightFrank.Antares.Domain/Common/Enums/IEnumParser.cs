namespace KnightFrank.Antares.Domain.Common.Enums
{
    using System;

    using KnightFrank.Antares.Dal.Model;

    public interface IEnumParser
    {
        TResult Parse<T, TResult>(Guid enumEntityId)
            where T : BaseEntityWithCode
            where TResult : struct;
    }
}