namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.Enums;

    using OfferType = KnightFrank.Antares.Domain.Common.Enums.OfferType;

    public class OfferEntityMapper : BaseEntityMapper<Offer, Tuple<OfferType, RequirementType>>
    {
        private readonly IControlsConfiguration<Tuple<OfferType, RequirementType>> offerControlsConfiguration;

        private readonly IReadGenericRepository<Dal.Model.Offer.OfferType> offerTypeRepository;

        private readonly IReadGenericRepository<Requirement> requirementRepository;

        public OfferEntityMapper(
            IControlsConfiguration<Tuple<OfferType, RequirementType>> offerControlsConfiguration,
            IReadGenericRepository<Requirement> requirementRepository,
            IReadGenericRepository<Dal.Model.Offer.OfferType> offerTypeRepository)
        {
            this.offerControlsConfiguration = offerControlsConfiguration;
            this.requirementRepository = requirementRepository;
            this.offerTypeRepository = offerTypeRepository;
        }

        public override Offer MapAllowedValues<TSource>(TSource source, Offer offer, PageType pageType)
        {
            Tuple<OfferType, RequirementType> configKey = this.GetConfigurationKey(offer);
            offer = base.MapAllowedValues(source, offer, this.offerControlsConfiguration, pageType, configKey);
            return offer;
        }

        public override Offer NullifyDisallowedValues(Offer offer, PageType pageType)
        {
            Tuple<OfferType, RequirementType> configKey = this.GetConfigurationKey(offer);
            offer = base.NullifyDisallowedValues(offer, this.offerControlsConfiguration, pageType, configKey);
            return offer;
        }

        private Tuple<OfferType, RequirementType> GetConfigurationKey(Offer offer)
        {
            OfferType offerType = this.GetOfferType(offer);
            RequirementType requirementType = this.GetRequirementType(offer);

            return new Tuple<OfferType, RequirementType>(offerType, requirementType);
        }

        private OfferType GetOfferType(Offer offer)
        {
            Dal.Model.Offer.OfferType offerType = offer.OfferType
                                                  ?? this.offerTypeRepository.Get().Single(x => x.Id == offer.OfferTypeId);
            return EnumExtensions.ParseEnum<OfferType>(offerType.EnumCode);
        }

        private RequirementType GetRequirementType(Offer offer)
        {
            OfferType offerType = this.GetOfferType(offer);
            return EnumMapper.GetRequirementType(offerType);

            // TODO remove above and replace below, after merge with branche 'residental-letting-req'
            /*Requirement requirement = offer.Requirement
                                      ?? this.requirementRepository.GetWithInclude(r => r.RequirementType)
                                             .Single(x => x.Id == offer.RequirementId);
            return  EnumExtensions.ParseEnum<RequirementType>(requirement.RequirementType.EnumCode);*/
        }
    }
}
