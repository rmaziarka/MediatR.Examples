namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Dal.Repository;

    using RequirementType = KnightFrank.Antares.Domain.Common.Enums.RequirementType;

    public class RequirementEntityMapper : BaseEntityMapper<Requirement, Tuple<RequirementType>>
    {       
        private readonly IControlsConfiguration<Tuple<RequirementType>> requirementControlsConfiguration;
        private readonly IReadGenericRepository<Dal.Model.Property.RequirementType> requirementTypeRepository;

        public RequirementEntityMapper(
            IControlsConfiguration<Tuple<RequirementType>> requirementControlsConfiguration,
            IReadGenericRepository<Dal.Model.Property.RequirementType> requirementTypeRepository)
        {
            this.requirementControlsConfiguration = requirementControlsConfiguration;
            this.requirementTypeRepository = requirementTypeRepository;
        }

        public override Requirement MapAllowedValues<TSource>(TSource source, Requirement requirement, PageType pageType)
        {
            Tuple<RequirementType> configKey = this.GetConfigurationKey(requirement);
            requirement = base.MapAllowedValues(source, requirement, this.requirementControlsConfiguration, pageType, configKey);
            return requirement;
        }

        public override Requirement NullifyDisallowedValues(Requirement requirement, PageType pageType)
        {
            Tuple<RequirementType> configKey = this.GetConfigurationKey(requirement);
            requirement = base.NullifyDisallowedValues(requirement, this.requirementControlsConfiguration, pageType, configKey);
            return requirement;
        }

        private Tuple<RequirementType> GetConfigurationKey(Requirement requirement)
        {
            Guid requirementTypeId = requirement.RequirementTypeId;
            Dal.Model.Property.RequirementType requirementType = requirement.RequirementType ?? this.requirementTypeRepository.Get().Single(x => x.Id == requirementTypeId);
            var requirementTypeEnum = EnumExtensions.ParseEnum<RequirementType>(requirementType.EnumCode);

            return new Tuple<RequirementType>(requirementTypeEnum);
        }
    }
}
