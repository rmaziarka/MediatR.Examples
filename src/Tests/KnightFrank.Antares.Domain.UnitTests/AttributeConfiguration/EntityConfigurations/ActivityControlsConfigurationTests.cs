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

    using Moq;

    using Xunit;

    [Collection("ActivityControlsConfiguration")]
    [Trait("FeatureTitle", "AttributeConfigurations")]
    public class ActivityControlsConfigurationTests
    {
        private ActivityControlsConfiguration activityControlsConfiguration;

        [Theory]
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

            // ToDo: Check & fix why there is not possible to verify expressions
            // this.CheckExpression(control.IsHiddenExpression, configItem.ControlHiddenExpression);
            // this.CheckExpression(control.IsReadonlyExpression, configItem.ControlReadonlyExpression);
        }

        private void CheckExpression(LambdaExpression actualExpression,
            Expression<Func<ActivityCommandBase, bool>> expectedExpression)
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
                new object[] { new ActivityControlsConfigurationItem(ControlCode.Negotiators, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, false) },

                new object[] { new ActivityControlsConfigurationItem(ControlCode.DisposalType, PageType.Create, PropertyType.Flat, ActivityType.FreeholdSale, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.DisposalType, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, true) },

                new object[] { new ActivityControlsConfigurationItem(ControlCode.KfValuationPrice, PageType.Create, PropertyType.Flat, ActivityType.FreeholdSale, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.VendorValuationPrice, PageType.Create, PropertyType.Flat, ActivityType.FreeholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.AgreedInitialMarketingPrice, PageType.Create, PropertyType.Flat, ActivityType.FreeholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.KfValuationPrice, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.VendorValuationPrice, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.AgreedInitialMarketingPrice, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, false) },

                new object[] { new ActivityControlsConfigurationItem(ControlCode.ShortKfValuationPrice, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.ShortVendorValuationPrice, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.ShortAgreedInitialMarketingPrice, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.LongKfValuationPrice, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, true) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.LongVendorValuationPrice, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.LongAgreedInitialMarketingPrice, PageType.Create, PropertyType.Flat, ActivityType.OpenMarketLetting, false) },

                new object[] { new ActivityControlsConfigurationItem(ControlCode.ServiceChargeAmount, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.ServiceChargeNote, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.GroundRentAmount, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.GroundRentNote, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, false) },

                new object[] { new ActivityControlsConfigurationItem(ControlCode.OtherCondition, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, false) },
                new object[] { new ActivityControlsConfigurationItem(ControlCode.Decoration, PageType.Create, PropertyType.Flat, ActivityType.LongLeaseholdSale, false) }
            };
        }

        private ActivityControlsConfiguration GetActivityControlsConfiguration()
        {
            var enumRepository = new Mock<IGenericRepository<EnumTypeItem>>();
            this.SetupEnumTypeRepository(enumRepository);
            return this.activityControlsConfiguration ?? (this.activityControlsConfiguration = new ActivityControlsConfiguration(enumRepository.Object));
        }

        private void SetupEnumTypeRepository(Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository)
        {
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                      {
                                          new EnumTypeItem()
                                          {
                                              Id = new Guid(),
                                              Code = ActivityStatus.MarketAppraisal.ToString(),
                                              EnumType = new KnightFrank.Antares.Dal.Model.Enum.EnumType
                                              {
                                                  Code = KnightFrank.Antares.Domain.Common.Enums.EnumType.ActivityStatus.ToString()
                                              }
                                          },
                                          new EnumTypeItem()
                                          {
                                              Id = new Guid(),
                                              Code = ActivityStatus.ForSaleUnavailable.ToString(),
                                              EnumType = new KnightFrank.Antares.Dal.Model.Enum.EnumType
                                              {
                                                  Code = KnightFrank.Antares.Domain.Common.Enums.EnumType.ActivityStatus.ToString()
                                              }
                                          },
                                          new EnumTypeItem()
                                          {
                                              Id = new Guid(),
                                              Code = ActivityStatus.ToLetUnavailable.ToString(),
                                              EnumType = new KnightFrank.Antares.Dal.Model.Enum.EnumType
                                              {
                                                  Code = KnightFrank.Antares.Domain.Common.Enums.EnumType.ActivityStatus.ToString()
                                              }
                                          },
                                          new EnumTypeItem()
                                          {
                                              Id = new Guid(),
                                              Code = ActivityMatchFlexPrice.MinimumPrice.ToString(),
                                              EnumType = new KnightFrank.Antares.Dal.Model.Enum.EnumType
                                              {
                                                  Code = KnightFrank.Antares.Domain.Common.Enums.EnumType.ActivityMatchFlexPrice.ToString()
                                              }
                                          },
                                          new EnumTypeItem()
                                          {
                                              Id = new Guid(),
                                              Code = ActivityMatchFlexPrice.Percentage.ToString(),
                                              EnumType = new KnightFrank.Antares.Dal.Model.Enum.EnumType
                                              {
                                                  Code = KnightFrank.Antares.Domain.Common.Enums.EnumType.ActivityMatchFlexPrice.ToString()
                                              }
                                          },
                                          new EnumTypeItem()
                                          {
                                              Id = new Guid(),
                                              Code = ActivityMatchFlexRent.MinimumRent.ToString(),
                                              EnumType = new KnightFrank.Antares.Dal.Model.Enum.EnumType
                                              {
                                                  Code = KnightFrank.Antares.Domain.Common.Enums.EnumType.ActivityMatchFlexRent.ToString()
                                              }
                                          },
                                          new EnumTypeItem()
                                          {
                                              Id = new Guid(),
                                              Code = ActivityMatchFlexRent.Percentage.ToString(),
                                              EnumType = new KnightFrank.Antares.Dal.Model.Enum.EnumType
                                              {
                                                  Code = KnightFrank.Antares.Domain.Common.Enums.EnumType.ActivityMatchFlexRent.ToString()
                                              }
                                          },
                                          new EnumTypeItem()
                                          {
                                              Id = new Guid(),
                                              Code = RentPaymentPeriod.Weekly.ToString(),
                                              EnumType = new KnightFrank.Antares.Dal.Model.Enum.EnumType
                                              {
                                                  Code = KnightFrank.Antares.Domain.Common.Enums.EnumType.RentPaymentPeriod.ToString()
                                              }
                                          },
                                          new EnumTypeItem()
                                          {
                                              Id = new Guid(),
                                              Code = RentPaymentPeriod.Monthly.ToString(),
                                              EnumType = new KnightFrank.Antares.Dal.Model.Enum.EnumType
                                              {
                                                  Code = KnightFrank.Antares.Domain.Common.Enums.EnumType.RentPaymentPeriod.ToString()
                                              }
                                          }
                                      }.Where(expr.Compile()));
        }
    }
}
