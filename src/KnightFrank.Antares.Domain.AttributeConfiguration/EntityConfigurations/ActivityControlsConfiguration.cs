namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;
    public class ActivityControlsConfiguration : BaseControlsConfiguration<PropertyType, ActivityType>
    {
        public override void DefineControls()
        {
            this.AddBaseControl(PageType.Create, ControlCode.Property, Field<CreateActivityCommand>.Create(x => x.PropertyId).Required().InnerField);
            this.AddBaseControl(PageType.Create, ControlCode.LeadNegotiator, Field<CreateActivityCommand>.Create(x => x.LeadNegotiatorId).Required().InnerField);

            this.AddControl(PageType.Create, ControlCode.ActivityStatus, Field<CreateActivityCommand>.CreateDictionary(x => x.ActivityStatusId, nameof(ActivityStatus)).Required().InnerField);
            this.AddControl(PageType.Create, ControlCode.Vendors, Field<CreateActivityCommand>.Create(x => x.ContactIds).InnerField);
            this.AddControl(PageType.Create, ControlCode.Landlords, Field<CreateActivityCommand>.Create(x => x.ContactIds).InnerField);
            this.AddControl(PageType.Create, ControlCode.ActivityType, Field<CreateActivityCommand>.Create(x => x.ActivityTypeId).Required().InnerField);
        }

        public override void DefineMappings()
        {
            this.Use(new List<ControlCode> { ControlCode.ActivityStatus, ControlCode.Landlords, ControlCode.ActivityType }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.GarageOnly,PropertyType.ParkingSpace, PropertyType.Houseboat
            }, ActivityType.OpenMarketLetting, PageType.Create));


            this.Use(new List<ControlCode> { ControlCode.ActivityStatus, ControlCode.Vendors, ControlCode.ActivityType }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.FarmEstate,
                PropertyType.GarageOnly,PropertyType.ParkingSpace, PropertyType.Land, PropertyType.Houseboat
            }, ActivityType.FreeholdSale, PageType.Create));

            this.Use(new List<ControlCode> { ControlCode.Vendors, ControlCode.ActivityType }, this.When(new List<PropertyType>
            {
                PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land
            }, ActivityType.LongLeaseholdSale, PageType.Create));

            this.Use(ControlCode.ActivityStatus, this.When(new List<PropertyType>
            {
                PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land
            }, ActivityType.LongLeaseholdSale, PageType.Create))
                .FieldHasAllowed<CreateActivityCommand, Guid, ActivityStatus>(
                    x => x.ActivityStatusId,
                    new List<ActivityStatus>
                        {
                            ActivityStatus.MarketAppraisal,
                            ActivityStatus.PreAppraisal
                        });
        }
    }
}
