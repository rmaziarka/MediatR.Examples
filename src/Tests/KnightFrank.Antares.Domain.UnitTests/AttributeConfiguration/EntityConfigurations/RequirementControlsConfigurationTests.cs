namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Domain.Requirement.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;

    using Xunit;

    [Collection("RequirementControlsConfiguration")]
    [Trait("FeatureTitle", "AttributeConfigurations")]
    public class RequirementControlsConfigurationTests
    {
        private RequirementControlsConfiguration activityControlsConfiguration;

        [Theory]
        [MemberData(nameof(GetCreateConfiguration))]
        public void Given_ConfigurationRequirementsForCreatePanel_When_ConfigurationIsInstantiated_Then_ConfigurationShouldBeCorrect(RequirementCreateControlsConfigurationItem configItem)
        {
            RequirementControlsConfiguration config = this.GetRequirementControlsConfiguration();
            var configKey = new Tuple<Tuple<RequirementType>, PageType>(new Tuple<RequirementType>(configItem.RequirementType), configItem.PageType);

            // check if there is a configuration for given page type, requirement type
            config.ControlPageConfiguration.ContainsKey(configKey).Should().BeTrue(configItem.ControlCode.ToString());

            IList<Tuple<Control, IList<IField>>> controlsForGivenConditions = config.ControlPageConfiguration[configKey];
            controlsForGivenConditions.Should().NotBeNullOrEmpty(configItem.ControlCode.ToString());

            Tuple<Control, IList<IField>> controlConfig = controlsForGivenConditions.First(x => x.Item1.ControlCode == configItem.ControlCode);
            Control control = controlConfig.Item1;
            IList<IField> fields = controlConfig.Item2;

            fields.Should().NotBeNullOrEmpty();
            fields.Select(x => x.InnerField.Required).All(x => x).Should().Be(configItem.IsRequired, configItem.ControlCode.ToString());

            this.CheckExpression(control.IsHiddenExpression, configItem.ControlHiddenExpression);
            this.CheckExpression(control.IsReadonlyExpression, configItem.ControlReadonlyExpression);
        }

        [Theory]
        [MemberData(nameof(GetDetailsConfiguration))]
        public void Given_ConfigurationRequirementsForDetailsView_When_ConfigurationIsInstantiated_Then_ConfigurationShouldBeCorrect(RequirementDetailsControlsConfigurationItem configItem)
        {
            RequirementControlsConfiguration config = this.GetRequirementControlsConfiguration();
            var configKey = new Tuple<Tuple<RequirementType>, PageType>(new Tuple<RequirementType>(configItem.RequirementType), configItem.PageType);

            // check if there is a configuration for given page type, requirement type
            config.ControlPageConfiguration.ContainsKey(configKey).Should().BeTrue(configItem.ControlCode.ToString());

            IList<Tuple<Control, IList<IField>>> controlsForGivenConditions = config.ControlPageConfiguration[configKey];
            controlsForGivenConditions.Should().NotBeNullOrEmpty(configItem.ControlCode.ToString());
        }

        private void CheckExpression(LambdaExpression actualExpression,
            Expression<Func<CreateRequirementCommand, object>> expectedExpression)
        {
            if (expectedExpression == null)
            {
                actualExpression.Should().BeNull();
            }
            else
            {
                actualExpression.Should().NotBeNull();
                actualExpression.ToString().ShouldBeEquivalentTo(expectedExpression.ToString());
            }
        }
        
        // ReSharper disable once UnusedMethodReturnValue.Local
        private static IEnumerable<object[]> GetCreateConfiguration()
        {
            return new[]
            {
                new object[] { new RequirementCreateControlsConfigurationItem(ControlCode.Requirement_RequirementType, PageType.Create,  RequirementType.ResidentialLetting, true) },
                new object[] { new RequirementCreateControlsConfigurationItem(ControlCode.Requirement_Description, PageType.Create,  RequirementType.ResidentialLetting, false) },
                new object[] { new RequirementCreateControlsConfigurationItem(ControlCode.Requirement_RentRange, PageType.Create,  RequirementType.ResidentialLetting, false) },
                new object[] { new RequirementCreateControlsConfigurationItem(ControlCode.Requirement_LocationRequirements, PageType.Create,  RequirementType.ResidentialLetting, true) },
                new object[] { new RequirementCreateControlsConfigurationItem(ControlCode.Requirement_Applicants, PageType.Create,  RequirementType.ResidentialLetting, true) },
                                   
                new object[] { new RequirementCreateControlsConfigurationItem(ControlCode.Requirement_RequirementType, PageType.Create, RequirementType.ResidentialSale, true) },
                new object[] { new RequirementCreateControlsConfigurationItem(ControlCode.Requirement_Description, PageType.Create, RequirementType.ResidentialSale, false) },
                new object[] { new RequirementCreateControlsConfigurationItem(ControlCode.Requirement_Applicants, PageType.Create, RequirementType.ResidentialSale, true) },
            };
        }
        private static IEnumerable<object[]> GetDetailsConfiguration()
        {
            return new[]
            {
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_CreationDate, PageType.Details,  RequirementType.ResidentialLetting) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_RequirementType, PageType.Details,  RequirementType.ResidentialLetting) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_Applicants, PageType.Details,  RequirementType.ResidentialLetting) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_Description, PageType.Details,  RequirementType.ResidentialLetting) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_RentRange, PageType.Details,  RequirementType.ResidentialLetting) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_LocationRequirements, PageType.Details,  RequirementType.ResidentialLetting) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_Viewings, PageType.Details,  RequirementType.ResidentialLetting) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_Attachments, PageType.Details,  RequirementType.ResidentialLetting) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_Offers, PageType.Details,  RequirementType.ResidentialLetting) },
                                   
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_CreationDate, PageType.Details, RequirementType.ResidentialSale) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_RequirementType, PageType.Details, RequirementType.ResidentialSale) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_Applicants, PageType.Details, RequirementType.ResidentialSale) },
                new object[] { new RequirementDetailsControlsConfigurationItem(ControlCode.Requirement_Description, PageType.Details, RequirementType.ResidentialSale) },
            };
        }

        private RequirementControlsConfiguration GetRequirementControlsConfiguration()
        {
            return this.activityControlsConfiguration ?? (this.activityControlsConfiguration = new RequirementControlsConfiguration());
        }
    }
}
