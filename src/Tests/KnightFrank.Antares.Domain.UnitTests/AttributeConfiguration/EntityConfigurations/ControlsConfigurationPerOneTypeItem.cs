namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Linq.Expressions;

    using Domain.AttributeConfiguration.Enums;

    public class ControlsConfigurationPerOneTypeItem<TEntity, TEnum1> where TEnum1 : struct
    {
        public readonly PageType PageType;

        public readonly TEnum1 Enum1;

        public readonly ControlCode ControlCode;

        public readonly bool? IsRequired;

        public readonly Expression<Func<TEntity, bool>> ControlHiddenExpression;

        public readonly Expression<Func<TEntity, bool>> ControlReadonlyExpression;

        public ControlsConfigurationPerOneTypeItem(
            PageType pageType,
            TEnum1 enum1,
            ControlCode controlCode,
            bool? isRequired = null,
            Expression<Func<TEntity, bool>> controlReadonlyExpression = null,
            Expression<Func<TEntity, bool>> controlHiddenExpression = null)
        {
            this.PageType = pageType;
            this.ControlCode = controlCode;
            this.Enum1 = enum1;
            this.IsRequired = isRequired;
            this.ControlReadonlyExpression = controlReadonlyExpression;
            this.ControlHiddenExpression = controlHiddenExpression;
        }

    }
}
