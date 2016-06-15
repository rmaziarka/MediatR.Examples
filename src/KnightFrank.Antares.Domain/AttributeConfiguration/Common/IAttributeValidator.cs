namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public interface IAttributeValidator<in TKey1, in TKey2>
    {
        void Validate(PageType pageType, TKey1 key1, TKey2 key2, object entity);
    }
}