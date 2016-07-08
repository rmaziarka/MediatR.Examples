namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions.Fields;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Tenancy.Commands;

    public class TenancyControlsConfiguration : ControlsConfigurationPerTwoTypes<TenancyType, RequirementType>
    {
        public TenancyControlsConfiguration()
        {
            this.Init();
        }

        public override void DefineControls()
        {
            this.DefineControlsForCreateAndUpdate();
            this.DefineControlsForDetails();
        }

        private void DefineControlsForCreateAndUpdate()
        {
            foreach (PageType pageType in new[] { PageType.Create, PageType.Update })
            {
                this.AddControl(pageType, ControlCode.Tenancy_Term,
                    Field<CreateTenancyCommand>.Create(x => x.Terms)
                                               .Required()
                                               .ExternalCollectionValidator(new CreateTenancyTermValidator()));
            }
        }

        private void DefineControlsForDetails()
        {
            this.AddControl(PageType.Details, ControlCode.Tenancy_Term, Field<CreateTenancyCommand>.Create(x => x.Terms));
        }

        public override void DefineMappings()
        {
            this.Use(new List<ControlCode> { ControlCode.Tenancy_Term }, 
                this.When(TenancyType.ResidentialLetting, RequirementType.ResidentialLetting, PageType.Create, PageType.Update, PageType.Details));
        }
    }
}
