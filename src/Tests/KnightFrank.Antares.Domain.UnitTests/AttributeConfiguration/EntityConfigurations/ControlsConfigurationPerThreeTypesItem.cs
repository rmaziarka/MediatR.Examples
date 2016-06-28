namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public class ControlsConfigurationPerThreeTypesItem<TEntity, TEnum1, TEnum2, TEnum3> :
        ControlsConfigurationPerTwoTypesItem<TEntity, TEnum1, TEnum2> where TEnum1 : struct where TEnum2 : struct where TEnum3 : struct
    {
        public readonly TEnum3 Enum3;

        public ControlsConfigurationPerThreeTypesItem(
            PageType pageType,
            TEnum1 enum1,
            TEnum2 enum2,
            TEnum3 enum3,
            ControlCode controlCode,
            bool? isRequired = null,
            Expression<Func<TEntity, object>> controlReadonlyExpression = null,
            Expression<Func<TEntity, object>> controlHiddenExpression = null)
            : base(pageType, enum1, enum2, controlCode, isRequired, controlReadonlyExpression, controlHiddenExpression)
        {
            this.Enum3 = enum3;
        }
    }
}
