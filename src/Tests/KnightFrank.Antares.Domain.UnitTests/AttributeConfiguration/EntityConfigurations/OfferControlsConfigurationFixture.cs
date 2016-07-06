namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Domain.UnitTests.Common.Extension;

    using OfferType = KnightFrank.Antares.Domain.Common.Enums.OfferType;

    public class OfferControlsConfigurationFixture
    {
        private static readonly ControlCode[] createNotRequiredControlCodes =
            {
                ControlCode.Offer_ExchangeDate,
                ControlCode.Offer_CompletionDate,
                ControlCode.Offer_SpecialConditions
            };

        private static readonly ControlCode[] createRequiredControlCodes =
            {
                ControlCode.Offer_Status, ControlCode.Offer_OfferDate
            };

        private static readonly ControlCode[] updateNotRequiredControlCodes = createRequiredControlCodes;

        private static readonly ControlCode[] updateRequiredControlCodes = createRequiredControlCodes;

        private static readonly ControlCode[] detailsControlCodes =
            updateRequiredControlCodes.Concat(updateNotRequiredControlCodes)
                                      .Concat(
                                          new[]
                                              {
                                                  ControlCode.Offer_Requirement, ControlCode.Offer_Activity,
                                                  ControlCode.Offer_Negotiator, ControlCode.Offer_CreatedDate,
                                                  ControlCode.Offer_LastModifiedDate
                                              });

        private static readonly ControlCode[] previewControlCodes =
            createRequiredControlCodes.Concat(createNotRequiredControlCodes)
                                      .Concat(new[] { ControlCode.Offer_Activity, ControlCode.Offer_Negotiator });

        private static readonly ControlCode[] residentialLettingControlCodes = { ControlCode.Offer_PricePerWeek };

        private static readonly ControlCode[] residentialSaleControlCodes = { ControlCode.Offer_Price };

        public static IEnumerable<object[]> GetCreateOfferConfiguration()
        {
            IEnumerable<object[]> data = BuildFixtureData<CreateOfferCommand>(
                new[] { PageType.Create },
                new[] { OfferType.ResidentialLetting },
                new[] { RequirementType.ResidentialLetting },
                residentialLettingControlCodes.Concat(createRequiredControlCodes),
                true);

            data =
                data.Concat(
                    BuildFixtureData<CreateOfferCommand>(
                        new[] { PageType.Create },
                        new[] { OfferType.ResidentialLetting },
                        new[] { RequirementType.ResidentialLetting },
                        residentialLettingControlCodes.Concat(createNotRequiredControlCodes)));

            data =
                data.Concat(
                    BuildFixtureData<CreateOfferCommand>(
                        new[] { PageType.Create },
                        new[] { OfferType.ResidentialSale },
                        new[] { RequirementType.ResidentialSale },
                        residentialSaleControlCodes.Concat(createRequiredControlCodes),
                        true));

            data =
                data.Concat(
                    BuildFixtureData<CreateOfferCommand>(
                        new[] { PageType.Create },
                        new[] { OfferType.ResidentialSale },
                        new[] { RequirementType.ResidentialSale },
                        residentialSaleControlCodes.Concat(createNotRequiredControlCodes)));

            return data;
        }

        public static IEnumerable<object[]> GetUpdateOfferConfiguration()
        {
            IEnumerable<object[]> data = BuildFixtureData<UpdateOfferCommand>(
                new[] { PageType.Update },
                new[] { OfferType.ResidentialLetting },
                new[] { RequirementType.ResidentialLetting },
                residentialLettingControlCodes.Concat(updateRequiredControlCodes),
                true);

            data =
                data.Concat(
                    BuildFixtureData<UpdateOfferCommand>(
                        new[] { PageType.Update },
                        new[] { OfferType.ResidentialLetting },
                        new[] { RequirementType.ResidentialLetting },
                        residentialLettingControlCodes.Concat(updateNotRequiredControlCodes)));

            data =
                data.Concat(
                    BuildFixtureData<UpdateOfferCommand>(
                        new[] { PageType.Update },
                        new[] { OfferType.ResidentialSale },
                        new[] { RequirementType.ResidentialSale },
                        residentialSaleControlCodes.Concat(updateRequiredControlCodes),
                        true));

            data =
                data.Concat(
                    BuildFixtureData<UpdateOfferCommand>(
                        new[] { PageType.Update },
                        new[] { OfferType.ResidentialSale },
                        new[] { RequirementType.ResidentialSale },
                        residentialSaleControlCodes.Concat(updateNotRequiredControlCodes).Concat(new []
                            {
                                ControlCode.Offer_Activity_Solicitor, ControlCode.Offer_Requirement_Solicitor
                            })));

            return data;
        }

        public static IEnumerable<object[]> GetDetailsOfferConfiguration()
        {
            IEnumerable<object[]> data = BuildFixtureData<Offer>(
                new[] { PageType.Details },
                new[] { OfferType.ResidentialLetting },
                new[] { RequirementType.ResidentialLetting },
                residentialLettingControlCodes.Concat(detailsControlCodes));

            data =
                data.Concat(
                    BuildFixtureData<Offer>(
                        new[] { PageType.Details },
                        new[] { OfferType.ResidentialSale },
                        new[] { RequirementType.ResidentialSale },
                        residentialSaleControlCodes.Concat(detailsControlCodes).Concat(new[]
                            {
                                ControlCode.Offer_Activity_Solicitor, ControlCode.Offer_Requirement_Solicitor
                            })));

            return data;
        }

        public static IEnumerable<object[]> GetPreviewOfferConfiguration()
        {
            IEnumerable<object[]> data = BuildFixtureData<Offer>(
                new[] { PageType.Preview },
                new[] { OfferType.ResidentialLetting },
                new[] { RequirementType.ResidentialLetting },
                residentialLettingControlCodes.Concat(previewControlCodes));

            data =
                data.Concat(
                    BuildFixtureData<Offer>(
                        new[] { PageType.Preview },
                        new[] { OfferType.ResidentialSale },
                        new[] { RequirementType.ResidentialSale },
                        residentialSaleControlCodes.Concat(previewControlCodes)));

            return data;
        }

        private static IEnumerable<object[]> BuildFixtureData<TEntity>(
            PageType[] pageTypes,
            OfferType[] offerTypes,
            RequirementType[] requirementTypes,
            ControlCode[] controleCodes)
        {
            return BuildFixtureData<TEntity>(pageTypes, offerTypes, requirementTypes, controleCodes, null, null);
        }

        private static IEnumerable<object[]> BuildFixtureData<TEntity>(
            PageType[] pageTypes,
            OfferType[] offerTypes,
            RequirementType[] requirementTypes,
            ControlCode[] controleCodes,
            bool? isRequired
            )
        {
            return BuildFixtureData<TEntity>(pageTypes, offerTypes, requirementTypes, controleCodes, isRequired, null);
        }

        private static IEnumerable<object[]> BuildFixtureData<TEntity>(
            PageType[] pageTypes,
            OfferType[] offerTypes,
            RequirementType[] requirementTypes,
            ControlCode[] controleCodes,
            bool? isRequired,
            Expression<Func<TEntity, bool>> readonlyExpression)
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
                                   isRequired,
                                   readonlyExpression)
                           };
        }
    }
}
