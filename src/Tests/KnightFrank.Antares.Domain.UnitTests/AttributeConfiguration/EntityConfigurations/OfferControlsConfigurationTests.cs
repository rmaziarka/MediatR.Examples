namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using Xunit;

    using OfferType = KnightFrank.Antares.Domain.Common.Enums.OfferType;

    [Collection("OfferControlsConfiguration")]
    [Trait("FeatureTitle", "OfferControlsConfigurations")]
    public class OfferControlsConfigurationTests : BaseControlsConfigurationTests<OfferControlsConfigurationFixture>
    {
        private OfferControlsConfiguration offerControlsConfiguration;

        private ControlsConfigurationPerTwoTypes<OfferType, RequirementType> GetControlsConfiguration()
        {
            return this.offerControlsConfiguration ?? (this.offerControlsConfiguration = new OfferControlsConfiguration());
        }

        [Theory]
        [MemberData(nameof(OfferControlsConfigurationFixture.GetCreateOfferConfiguration),
            MemberType = typeof(OfferControlsConfigurationFixture))]
        public void
            Given_ConfigurationForCreate_When_ConfigurationIsInstantiated_Then_ConfigurationShouldBeCorrect(
            ControlsConfigurationPerTwoTypesItem<CreateOfferCommand, OfferType, RequirementType> controlConfigurationItem)
        {
            this.ControlsConfigurationPerTwoTypesItemTest(controlConfigurationItem, this.GetControlsConfiguration());
        }


        [Theory]
        [MemberData(nameof(OfferControlsConfigurationFixture.GetUpdateOfferConfiguration),
            MemberType = typeof(OfferControlsConfigurationFixture))]
        public void
            Given_ConfigurationForUpdate_When_ConfigurationIsInstantiated_Then_ConfigurationShouldBeCorrect(
            ControlsConfigurationPerTwoTypesItem<UpdateOfferCommand, OfferType, RequirementType> controlConfigurationItem)
        {
            this.ControlsConfigurationPerTwoTypesItemTest(controlConfigurationItem, this.GetControlsConfiguration());
        }


        [Theory]
        [MemberData(nameof(OfferControlsConfigurationFixture.GetDetailsOfferConfiguration), MemberType = typeof(OfferControlsConfigurationFixture))]
        [MemberData(nameof(OfferControlsConfigurationFixture.GetPreviewOfferConfiguration), MemberType = typeof(OfferControlsConfigurationFixture))]
        public void
            Given_ConfigurationForRead_When_ConfigurationIsInstantiated_Then_ConfigurationShouldBeCorrect(
            ControlsConfigurationPerTwoTypesItem<Offer, OfferType, RequirementType> controlConfigurationItem)
        {
            this.ControlsConfigurationPerTwoTypesItemTest(controlConfigurationItem, this.GetControlsConfiguration());
        }
    }
}
