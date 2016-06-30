namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Domain.Requirement.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.Enums;


    public class RequirementCreateControlsConfigurationItem
    {
        public ControlCode ControlCode;
        public PageType PageType;
        public RequirementType RequirementType;
        public Expression<Func<CreateRequirementCommand, bool>> ControlReadonlyExpression;
        public Expression<Func<CreateRequirementCommand, bool>> ControlHiddenExpression;
        public bool IsRequired;

        public RequirementCreateControlsConfigurationItem(ControlCode controlCode, PageType pageType, RequirementType requrementType, bool isRequired, Expression<Func<CreateRequirementCommand, bool>> controlReadonlyExpression = null, Expression<Func<CreateRequirementCommand, bool>> controlHiddenExpression = null)
        {
            this.ControlCode = controlCode;
            this.PageType = pageType;
            this.RequirementType = requrementType;
            this.IsRequired = isRequired;
            this.ControlReadonlyExpression = controlReadonlyExpression;
            this.ControlHiddenExpression = controlHiddenExpression;
        }
    }
}