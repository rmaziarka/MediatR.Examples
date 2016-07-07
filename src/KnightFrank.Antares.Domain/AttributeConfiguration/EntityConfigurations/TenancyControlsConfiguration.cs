namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Enums;

    public class TenancyControlsConfiguration : ControlsConfigurationPerOneType<RequirementType>
    {
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public TenancyControlsConfiguration(IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.Init();
        }

        public override void DefineControls()
        {
            throw new NotImplementedException();
        }

        public override void DefineMappings()
        {
            throw new NotImplementedException();
        }
    }
}
