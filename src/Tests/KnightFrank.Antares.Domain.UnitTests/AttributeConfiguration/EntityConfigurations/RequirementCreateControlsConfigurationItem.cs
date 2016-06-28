namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Domain.Requirement.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.Enums;


    public class RequirementCreateControlsConfigurationItem : RequirementDetailsControlsConfigurationItem
    {
        public Expression<Func<CreateRequirementCommand, object>> ControlReadonlyExpression;
        public Expression<Func<CreateRequirementCommand, object>> ControlHiddenExpression;
        public bool IsRequired;

        public RequirementCreateControlsConfigurationItem(ControlCode controlCode, PageType pageType, RequirementType requrementType, bool isRequired, Expression<Func<CreateRequirementCommand, object>> controlReadonlyExpression = null, Expression<Func<CreateRequirementCommand, object>> controlHiddenExpression = null)
            : base(controlCode, pageType, requrementType)
        {
            this.IsRequired = isRequired;
            this.ControlReadonlyExpression = controlReadonlyExpression;
            this.ControlHiddenExpression = controlHiddenExpression;
        }
    }
}