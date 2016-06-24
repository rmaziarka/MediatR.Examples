namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Common.Validator;
    using KnightFrank.Antares.Domain.Offer.Commands;

    public class OfferControlsConfiguration : ControlsConfigurationPerTwoTypes<OfferType, RequirementType>
    {
        public override void DefineControls()
        {
            this.DefineControlsForCreate();
        }

        private void DefineControlsForCreate()
        {
            this.AddControl(PageType.Create, ControlCode.Offer_Activity, Field<CreateOfferCommand>.Create(x => x.ActivityId).Required());
            this.AddControl(PageType.Create, ControlCode.Offer_Requirement, Field<CreateOfferCommand>.Create(x => x.RequirementId).Required());
            this.AddControl(PageType.Create, ControlCode.Offer_OfferType, Field<CreateOfferCommand>.Create(x => x.OfferTypeId).Required());
            this.AddControl(PageType.Create, ControlCode.Offer_Status, Field<CreateOfferCommand>.CreateDictionary(x => x.StatusId, nameof(OfferStatus)).Required());
            this.AddControl(PageType.Create, ControlCode.Offer_Price, Field<CreateOfferCommand>.Create(x => x.Price).Required().GreaterThan(0));
            this.AddControl(PageType.Create, ControlCode.Offer_PricePerWeek, Field<CreateOfferCommand>.Create(x => x.PricePerWeek).Required().GreaterThan(0));
            this.AddControl(PageType.Create, ControlCode.Offer_OfferDate, Field<CreateOfferCommand>.Create(x => x.OfferDate).Required().ExternalValidator(new DateInPastValidator(nameof(CreateOfferCommand.OfferDate))));
            this.AddControl(PageType.Create, ControlCode.Offer_ExchangeDate, Field<CreateOfferCommand>.Create(x => x.ExchangeDate).ExternalValidator(new DateInFutureValidator(nameof(CreateOfferCommand.ExchangeDate))));
            this.AddControl(PageType.Create, ControlCode.Offer_CompletionDate, Field<CreateOfferCommand>.Create(x => x.CompletionDate).ExternalValidator(new DateInFutureValidator(nameof(CreateOfferCommand.CompletionDate))));
            this.AddControl(PageType.Create, ControlCode.Offer_SpecialConditions, Field<CreateOfferCommand>.CreateText(x => x.SpecialConditions, 4000).Required());
        }

        public override void DefineMappings()
        {
            this.Use(
                new List<ControlCode>
                    {
                        ControlCode.Offer_Activity,
                        ControlCode.Offer_Status,
                        ControlCode.Offer_Price,
                        ControlCode.Offer_OfferDate,
                        ControlCode.Offer_SpecialConditions,
                        ControlCode.Offer_ExchangeDate,
                        ControlCode.Offer_CompletionDate
                    },
                this.When(OfferType.ResidentalSale, RequirementType.ResidentalSale, PageType.Create));

            this.Use(
                new List<ControlCode>
                    {
                        ControlCode.Offer_Activity,
                        ControlCode.Offer_Status,
                        ControlCode.Offer_PricePerWeek,
                        ControlCode.Offer_OfferDate,
                        ControlCode.Offer_SpecialConditions,
                        ControlCode.Offer_ExchangeDate,
                        ControlCode.Offer_CompletionDate
                    },
                this.When(OfferType.ResidentalLetting, RequirementType.ResidentalLetting, PageType.Create));
        }
    }
}
