namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using OfferType = KnightFrank.Antares.Domain.Common.Enums.OfferType;

    public class OfferControlsConfigurationFixture
    {
        public static IEnumerable<object[]> GetCreateOfferConfiguration()
        {
            IEnumerable<object[]> data = BuildFixtureData<CreateOfferCommand>(
                new[] { PageType.Create },
                new[] { OfferType.ResidentialLetting },
                new[] { RequirementType.ResidentialLetting },
                new[] { ControlCode.Offer_Status },
                true);

            return data;
        }

        public static IEnumerable<object[]> GetUpdateOfferConfiguration()
        {
            IEnumerable<object[]> data = BuildFixtureData<UpdateOfferCommand>(
                new[] { PageType.Update },
                new[] { OfferType.ResidentialLetting },
                new[] { RequirementType.ResidentialLetting },
                new[] { ControlCode.Offer_Status },
                true);

            return data;
        }

        public static IEnumerable<object[]> GetDetailsOfferConfiguration()
        {
            IEnumerable<object[]> data = BuildFixtureData<Offer>(
                new[] { PageType.Details },
                new[] { OfferType.ResidentialLetting },
                new[] { RequirementType.ResidentialLetting },
                new[] { ControlCode.Offer_Status });

            return data;
        }

        public static IEnumerable<object[]> GetPreviewOfferConfiguration()
        {
            IEnumerable<object[]> data = BuildFixtureData<Offer>(
                new[] { PageType.Preview },
                new[] { OfferType.ResidentialLetting },
                new[] { RequirementType.ResidentialLetting },
                new[] { ControlCode.Offer_Status });

            return data;
        }

        private static IEnumerable<object[]> BuildFixtureData<TEntity>(
            PageType[] pageTypes,
            OfferType[] offerTypes,
            RequirementType[] requirementTypes,
            ControlCode[] controleCodes)
        {
            return BuildFixtureData<TEntity>(pageTypes, offerTypes, requirementTypes, controleCodes, null);
        }

        private static IEnumerable<object[]> BuildFixtureData<TEntity>(
            PageType[] pageTypes,
            OfferType[] offerTypes,
            RequirementType[] requirementTypes,
            ControlCode[] controleCodes,
            bool? isRequired)
        {
            return from pageType in pageTypes
                   from offerType in offerTypes
                   from requirementType in requirementTypes
                   from controleCode in controleCodes
                   select
                       new object[]
                           {
                               new ControlsConfigurationPerTwoTypesItem<TEntity, OfferType, RequirementType>(
                                   pageType,
                                   offerType,
                                   requirementType,
                                   controleCode,
                                   isRequired)
                           };
        }
    }
}
