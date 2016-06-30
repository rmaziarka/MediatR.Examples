namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    using RequirementType = KnightFrank.Antares.Domain.Common.Enums.RequirementType;

    public class RequirementDetailsControlsConfigurationItem
    {
        public ControlCode ControlCode;
        public PageType PageType;
        public RequirementType RequirementType;
        public Expression<Func<Requirement, bool>> ControlHiddenExpression;

        public RequirementDetailsControlsConfigurationItem(ControlCode controlCode, PageType pageType, RequirementType requrementType, Expression<Func<Requirement, bool>> controlReadonlyExpression = null)
        {
            this.ControlCode = controlCode;
            this.PageType = pageType;
            this.RequirementType = requrementType;
            this.ControlHiddenExpression = controlReadonlyExpression;
        }
    }
}