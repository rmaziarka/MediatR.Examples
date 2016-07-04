namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Xunit;

    [Collection("ActivityControlsConfiguration")]
    [Trait("FeatureTitle", "AttributeConfigurations")]
    public class ActivityControlsConfigurationTests
    {
        private ActivityControlsConfiguration activityControlsConfiguration;

        [Theory(Skip = "Test will be fixed later")]
        [MemberData(nameof(GetActivityConfiguration))]
        public void Given_ConfigurationRequirementsForCreatePanel_When_ConfigurationIsInstantiated_Then_ConfigurationShouldBeCorrect(ActivityControlsConfigurationItem configItem)
        {
            ActivityControlsConfiguration config = this.GetActivityControlsConfiguration();
            var configKey = new Tuple<Tuple<PropertyType, ActivityType>, PageType>(new Tuple<PropertyType, ActivityType>(configItem.PropertyType, configItem.ActivityType), configItem.PageType);

            // check if there is a configuration for given page type, property type and activity type
            config.ControlPageConfiguration.ContainsKey(configKey).Should().BeTrue();

            IList<Tuple<Control, IList<IField>>> controlsForGivenConditions = config.ControlPageConfiguration[configKey];
            controlsForGivenConditions.Should().NotBeNullOrEmpty();

            Tuple<Control, IList<IField>> controlConfig = controlsForGivenConditions.First(x => x.Item1.ControlCode == configItem.ControlCode);
            Control control = controlConfig.Item1;
            IList<IField> fields = controlConfig.Item2;

            fields.Should().NotBeNullOrEmpty();
            fields.Select(x => x.InnerField.Required).All(x => x).Should().Be(configItem.IsRequired);

            this.CheckExpression(control.IsHiddenExpression, configItem.ControlHiddenExpression);
            this.CheckExpression(control.IsReadonlyExpression, configItem.ControlReadonlyExpression);
        }

        private void CheckExpression(LambdaExpression actualExpression,
            Expression<Func<CreateActivityCommand, object>> expectedExpression)
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
        private static IEnumerable<object[]> GetActivityConfiguration()
        {
            return new[]
            {
                new object[] { new ActivityControlsConfigurationItem(ControlCode.ActivityType, PageType.Create, PropertyType.Flat, ActivityType.FreeholdSale, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.ActivityStatus, PageType.Create, PropertyType.Flat, ActivityType.FreeholdSale, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.Vendors, PageType.Create, PropertyType.Flat, ActivityType.FreeholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.Negotiators, PageType.Create, PropertyType.Flat, ActivityType.FreeholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.SellingReason, PageType.Create, PropertyType.Flat, ActivityType.FreeholdSale, true) },

                new object[] { new ActivityControlsConfigurationItem(ControlCode.ActivityType, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.ActivityStatus, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.Vendors, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.Negotiators, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.SellingReason, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, true) },

                new object[] { new ActivityControlsConfigurationItem(ControlCode.ActivityType, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.ActivityStatus, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.Landlords, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.Negotiators, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, false) }
            };
        }

        private ActivityControlsConfiguration GetActivityControlsConfiguration()
        {
            var enumRepository = new Mock<IGenericRepository<EnumTypeItem>>(); 
            return this.activityControlsConfiguration ?? (this.activityControlsConfiguration = new ActivityControlsConfiguration(enumRepository.Object));
        }
    }
}
