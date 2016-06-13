namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public interface IControlsConfiguration<in TKey1, in TKey2>
    {
        IList<InnerFieldState> GetInnerFieldsState(PageType pageType, TKey1 key1, TKey2 key2, object entity);
    }
}