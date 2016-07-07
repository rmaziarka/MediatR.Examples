namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.Enums;

    public class ActivityControlsConfigurationItem
    {
        public ControlCode ControlCode;
        public PageType PageType;
        public PropertyType PropertyType;
        public ActivityType ActivityType;
        public Expression<Func<ActivityCommandBase, bool>> ControlReadonlyExpression;
        public Expression<Func<ActivityCommandBase, bool>> ControlHiddenExpression;
        public bool IsRequired;

        public ActivityControlsConfigurationItem(ControlCode controlCode, PageType pageType, PropertyType propertyType, ActivityType activityType, bool isRequired, Expression<Func<ActivityCommandBase, bool>> controlReadonlyExpression = null, Expression<Func<ActivityCommandBase, bool>> controlHiddenExpression = null)
        {
            this.ControlCode = controlCode;
            this.PageType = pageType;
            this.PropertyType = propertyType;
            this.ActivityType = activityType;
            this.IsRequired = isRequired;
            this.ControlReadonlyExpression = controlReadonlyExpression;
            this.ControlHiddenExpression = controlHiddenExpression;
        }
    }
}