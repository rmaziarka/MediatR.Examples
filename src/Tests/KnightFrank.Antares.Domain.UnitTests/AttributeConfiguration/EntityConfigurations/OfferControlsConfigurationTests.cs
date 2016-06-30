namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using Moq;

    using Xunit;

    using OfferType = KnightFrank.Antares.Domain.Common.Enums.OfferType;

    [Collection("OfferControlsConfiguration")]
    [Trait("FeatureTitle", "OfferControlsConfigurations")]
    public class OfferControlsConfigurationTests : BaseControlsConfigurationTests<OfferControlsConfigurationFixture>
    {
        private readonly OfferControlsConfiguration offerControlsConfiguration;

        public OfferControlsConfigurationTests()
        {
            var enumtypeItemRepository = new Mock<IGenericRepository<EnumTypeItem>>();
            enumtypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns(new[] { new EnumTypeItem { Id = Guid.NewGuid() } });

            this.offerControlsConfiguration = new OfferControlsConfiguration(enumtypeItemRepository.Object);
        }

        [Theory]
        [MemberData(nameof(OfferControlsConfigurationFixture.GetCreateOfferConfiguration),
            MemberType = typeof(OfferControlsConfigurationFixture))]
        public void Given_ConfigurationForCreate_When_ConfigurationIsInstantiated_Then_ConfigurationShouldBeCorrect(
            ControlsConfigurationPerTwoTypesItem<CreateOfferCommand, OfferType, RequirementType> controlConfigurationItem)
        {
            this.ControlsConfigurationPerTwoTypesItemTest(controlConfigurationItem, this.offerControlsConfiguration);
        }

        [Theory]
        [MemberData(nameof(OfferControlsConfigurationFixture.GetUpdateOfferConfiguration),
            MemberType = typeof(OfferControlsConfigurationFixture))]
        public void Given_ConfigurationForUpdate_When_ConfigurationIsInstantiated_Then_ConfigurationShouldBeCorrect(
            ControlsConfigurationPerTwoTypesItem<UpdateOfferCommand, OfferType, RequirementType> controlConfigurationItem)
        {
            this.ControlsConfigurationPerTwoTypesItemTest(controlConfigurationItem, this.offerControlsConfiguration);
        }

        [Theory]
        [MemberData(nameof(OfferControlsConfigurationFixture.GetDetailsOfferConfiguration),
            MemberType = typeof(OfferControlsConfigurationFixture))]
        [MemberData(nameof(OfferControlsConfigurationFixture.GetPreviewOfferConfiguration),
            MemberType = typeof(OfferControlsConfigurationFixture))]
        public void Given_ConfigurationForRead_When_ConfigurationIsInstantiated_Then_ConfigurationShouldBeCorrect(
            ControlsConfigurationPerTwoTypesItem<Offer, OfferType, RequirementType> controlConfigurationItem)
        {
            this.ControlsConfigurationPerTwoTypesItemTest(controlConfigurationItem, this.offerControlsConfiguration);
        }
    }
}
