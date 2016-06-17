namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;
    using System.Collections;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public interface IAttributeValidator<in TKey> where TKey : IStructuralEquatable, IStructuralComparable, IComparable
    {
        void Validate(PageType pageType, TKey key, object entity);
    }
}