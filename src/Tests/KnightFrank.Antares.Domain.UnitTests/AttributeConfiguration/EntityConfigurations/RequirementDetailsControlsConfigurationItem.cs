namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.Enums;

    public class RequirementDetailsControlsConfigurationItem
    {
        public ControlCode ControlCode;
        public PageType PageType;
        public RequirementType RequirementType;

        public RequirementDetailsControlsConfigurationItem(ControlCode controlCode, PageType pageType, RequirementType requrementType)
        {
            this.ControlCode = controlCode;
            this.PageType = pageType;
            this.RequirementType = requrementType;
        }
    }
}