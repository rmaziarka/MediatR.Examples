namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public interface IControlsConfiguration<in TKey> where TKey : IStructuralEquatable, IStructuralComparable, IComparable
    {
        IList<InnerFieldState> GetInnerFieldsState(PageType pageType, TKey key, object entity);
    }
}