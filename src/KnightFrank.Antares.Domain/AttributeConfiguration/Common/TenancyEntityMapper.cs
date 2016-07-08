namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Dal.Repository;

    using TenancyType = KnightFrank.Antares.Domain.Common.Enums.TenancyType;

    public class TenancyEntityMapper : BaseEntityMapper<Tenancy, Tuple<TenancyType>>
    {
        private readonly IControlsConfiguration<Tuple<TenancyType>> tenancyTermControlsConfiguration;
        private readonly IGenericRepository<Dal.Model.Tenancy.TenancyType> tenancyTypeRepository;

        public TenancyEntityMapper(IControlsConfiguration<Tuple<TenancyType>> tenancyTermControlsConfiguration,
             IGenericRepository<Dal.Model.Tenancy.TenancyType> tenancyTypeRepository)
        {
            this.tenancyTermControlsConfiguration = tenancyTermControlsConfiguration;
            this.tenancyTypeRepository = tenancyTypeRepository;
        }

        public override Tenancy MapAllowedValues<TSource>(TSource source, Tenancy tenancy, PageType pageType)
        {
            Tuple<TenancyType> configKey = this.GetConfigurationKey(tenancy);
            tenancy = base.MapAllowedValues(source, tenancy, this.tenancyTermControlsConfiguration, pageType, configKey);
            return tenancy;
        }

        public override Tenancy NullifyDisallowedValues(Tenancy tenancy, PageType pageType)
        {
            Tuple<TenancyType> configKey = this.GetConfigurationKey(tenancy);
            tenancy = base.NullifyDisallowedValues(tenancy, this.tenancyTermControlsConfiguration, pageType, configKey);
            return tenancy;
        }

        private Tuple<TenancyType> GetConfigurationKey(Tenancy tenancy)
        {
            Dal.Model.Tenancy.TenancyType tenancyType = tenancy.TenancyType ?? 
                this.tenancyTypeRepository.GetById(tenancy.TenancyTypeId);

            var requirementTypeEnum = EnumExtensions.ParseEnum<TenancyType>(tenancyType.EnumCode);

            return new Tuple<TenancyType>(requirementTypeEnum);
        }
    }
}
