namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Dal.Repository;

    using TenancyType = KnightFrank.Antares.Domain.Common.Enums.TenancyType;

    public class TenancyEntityMapper : BaseEntityMapper<Tenancy, Tuple<TenancyType>>
    {
        private readonly IControlsConfiguration<Tuple<TenancyType>> tenancyTermControlsConfiguration;
        private readonly IReadGenericRepository<Requirement> requirementRepository;

        public TenancyEntityMapper(IControlsConfiguration<Tuple<TenancyType>> tenancyTermControlsConfiguration,
             IReadGenericRepository<Requirement> requirementRepository)
        {
            this.tenancyTermControlsConfiguration = tenancyTermControlsConfiguration;
            this.requirementRepository = requirementRepository;
        }

        public override Tenancy MapAllowedValues<TSource>(TSource source, Tenancy tenancy, PageType pageType)
        {
            Tuple<TenancyType> configKey = this.GetConfigurationKey(tenancy);
            tenancy = base.MapAllowedValues(source, tenancy, this.tenancyTermControlsConfiguration, pageType, configKey);
            return tenancy;
        }

        public override Tenancy NullifyDisallowedValues(Tenancy requirement, PageType pageType)
        {
            Tuple<TenancyType> configKey = this.GetConfigurationKey(requirement);
            requirement = base.NullifyDisallowedValues(requirement, this.tenancyTermControlsConfiguration, pageType, configKey);
            return requirement;
        }

        private Tuple<TenancyType> GetConfigurationKey(Tenancy tenancy)
        {
            // TODO use tenancy type
            Requirement requirement = tenancy.Requirement ?? 
                this.requirementRepository.GetWithInclude(x => x.RequirementType).Single(x => x.Id == tenancy.RequirementId);

            var requirementTypeEnum = EnumExtensions.ParseEnum<TenancyType>(requirement.RequirementType.EnumCode);

            return new Tuple<TenancyType>(requirementTypeEnum);
        }
    }
}
