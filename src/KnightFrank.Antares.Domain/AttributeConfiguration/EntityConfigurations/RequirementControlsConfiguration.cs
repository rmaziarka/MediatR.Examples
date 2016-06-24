namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions.Fields;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Commands;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using RequirementType = KnightFrank.Antares.Domain.Common.Enums.RequirementType;

    public class RequirementControlsConfiguration : ControlsConfigurationPerOneType<RequirementType>
    {
        public override void DefineControls()
        {
            this.DefineControlsForCreate();
            this.DefineControlsForDetails();
        }

        private void DefineControlsForCreate()
        {            
            this.AddControl(PageType.Create, ControlCode.Requirement_RequirementType,
                Field<CreateRequirementCommand>.CreateDictionary(x => x.RequirementTypeId, nameof(RequirementType)).Required());
            this.AddControl(PageType.Create, ControlCode.Requirement_Description,
                Field<CreateRequirementCommand>.CreateText(x => x.Description, 4000));
            this.AddControl(PageType.Create, ControlCode.Requirement_RentRange, new List<IField>
            {
                Field<CreateRequirementCommand>.Create(x => x.RentMin).GreaterThan(0).LessThanOrEqualTo(x => x.RentMax),
                Field<CreateRequirementCommand>.Create(x => x.RentMax).GreaterThanOrEqualTo(x => x.RentMin)
            });
            this.AddControl(PageType.Create, ControlCode.Requirement_LocationRequirements, Field<CreateRequirementCommand>.Create(x => x.Address).Required().ExternalValidator(new CreateOrUpdateAddressValidator()));
            this.AddControl(PageType.Create, ControlCode.Requirement_Applicants,
                Field<CreateRequirementCommand>.Create(x => x.ContactIds).Required());
        }

        private void DefineControlsForDetails()
        {
            this.AddControl(PageType.Details, ControlCode.Requirement_CreationDate, Field<Requirement>.Create(x => x.CreateDate));
            this.AddControl(PageType.Details, ControlCode.Requirement_RequirementType,
                Field<Requirement>.CreateDictionary(x => x.RequirementTypeId, nameof(RequirementType)).Required());
            this.AddControl(PageType.Details, ControlCode.Requirement_Description,
                Field<Requirement>.CreateText(x => x.Description, 4000));
            this.AddControl(PageType.Details, ControlCode.Requirement_RentRange, new List<IField>
            {                
                Field<Requirement>.Create(x => x.RentMin),
                Field<Requirement>.Create(x => x.RentMax)
            });
            this.AddControl(PageType.Details, ControlCode.Requirement_LocationRequirements, Field<Requirement>.Create(x => x.Address));
            this.AddControl(PageType.Details, ControlCode.Requirement_Applicants, Field<Requirement>.Create(x => x.Contacts));
            this.AddControl(PageType.Details, ControlCode.Requirement_Viewings, Field<Requirement>.Create(x => x.Viewings));
            this.AddControl(PageType.Details, ControlCode.Requirement_Attachments, Field<Requirement>.Create(x => x.Attachments));
            this.AddControl(PageType.Details, ControlCode.Requirement_Offers, Field<Requirement>.Create(x => x.Offers));            
        }        

        public override void DefineMappings()
        {
            this.Use(
                new List<ControlCode>
                {                    
                    ControlCode.Requirement_RequirementType,
                    ControlCode.Requirement_Applicants,
                    ControlCode.Requirement_Description
                }, this.ForAll(PageType.Create));

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.CreationDate,
                    ControlCode.Requirement_RequirementType,
                    ControlCode.Requirement_Applicants,
                    ControlCode.Requirement_Description
                }, this.ForAll(PageType.Details));

            this.Use(new List<ControlCode>
            {
                ControlCode.Requirement_RentRange,
                ControlCode.Requirement_LocationRequirements,
            }, this.When(new List<RequirementType>
            {
                RequirementType.ResidentialLetting
            }, PageType.Create));

            this.Use(new List<ControlCode>
            {
                ControlCode.CreationDate,
                ControlCode.Requirement_RentRange,
                ControlCode.Requirement_LocationRequirements,
                ControlCode.Viewings,
                ControlCode.Requirement_Attachments,
                ControlCode.Offers
            }, this.When(new List<RequirementType>
            {
                RequirementType.ResidentialLetting
            }, PageType.Details));
        }
    }
}
