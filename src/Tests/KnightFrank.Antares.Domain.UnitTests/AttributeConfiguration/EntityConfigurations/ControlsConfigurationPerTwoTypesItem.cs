namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public class ControlsConfigurationPerTwoTypesItem<TEntity, TEnum1, TEnum2> :
        ControlsConfigurationPerOneTypeItem<TEntity, TEnum1>
        where TEnum1 : struct where TEnum2 : struct
    {
        public readonly TEnum2 Enum2;

        public ControlsConfigurationPerTwoTypesItem(
            PageType pageType,
            TEnum1 enum1,
            TEnum2 enum2,
            ControlCode controlCode,
            bool? isRequired = null,
            Expression<Func<TEntity, bool>> controlReadonlyExpression = null,
            Expression<Func<TEntity, bool>> controlHiddenExpression = null)
            : base(pageType, enum1, controlCode, isRequired, controlReadonlyExpression, controlHiddenExpression)
        {
            this.Enum2 = enum2;
        }
    }
}
