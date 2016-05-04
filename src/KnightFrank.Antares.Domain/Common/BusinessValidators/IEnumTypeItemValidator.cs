namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;

    using KnightFrank.Antares.Domain.Common.Specifications;

    public interface IEnumTypeItemValidator
    {
        void ItemExists(EnumTypeExists specification, Guid enumTypeItemId);
    }
}