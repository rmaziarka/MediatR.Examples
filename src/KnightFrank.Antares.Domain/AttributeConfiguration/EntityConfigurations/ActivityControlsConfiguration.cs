namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;

    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;

    public class ActivityControlsConfiguration : ControlsConfigurationPerTwoTypes<PropertyType, ActivityType>
    {
        public override void DefineControls()
        {
            this.DefineControlsForCreate();
            this.DefineControlsForDetailsView();
            this.DefineControlsForPreview();
            this.DefineControlsForEdit();
        }

        private void DefineControlsForPreview()
        {
            this.AddControl(PageType.Preview, ControlCode.ActivityStatus, Field<Activity>.Create(x => x.ActivityStatusId, x => x.ActivityStatus));
            this.AddControl(PageType.Preview, ControlCode.Vendors, Field<Activity>.Create(x => x.Contacts));
            this.AddControl(PageType.Preview, ControlCode.Landlords, Field<Activity>.Create(x => x.Contacts));
            this.AddControl(PageType.Preview, ControlCode.ActivityType, Field<Activity>.Create(x => x.ActivityTypeId, x => x.ActivityType));
            this.AddControl(PageType.Preview, ControlCode.CreationDate, Field<Activity>.Create(x => x.CreatedDate));
        }

        private void DefineControlsForDetailsView()
        {
            this.AddControl(PageType.Details, ControlCode.ActivityType, Field<Activity>.Create(x => x.ActivityTypeId, x => x.ActivityType));
            this.AddControl(PageType.Details, ControlCode.ActivityStatus, Field<Activity>.Create(x => x.ActivityStatusId, x => x.ActivityStatus));
            this.AddControl(PageType.Details, ControlCode.Vendors, Field<Activity>.Create(x => x.Contacts));
            this.AddControl(PageType.Details, ControlCode.Property, Field<Activity>.Create(x => x.PropertyId, x => x.Property));
            this.AddControl(PageType.Details, ControlCode.Negotiators, Field<Activity>.Create(x => x.ActivityUsers));
            this.AddControl(PageType.Details, ControlCode.Landlords, Field<Activity>.Create(x => x.Contacts));
            this.AddControl(PageType.Details, ControlCode.Departments, Field<Activity>.Create(x => x.ActivityDepartments));
            this.AddControl(PageType.Details, ControlCode.AskingPrice, Field<Activity>.Create(x => x.AskingPrice));
            this.AddControl(PageType.Details, ControlCode.ShortLetPricePerWeek, Field<Activity>.Create(x => x.ShortLetPricePerWeek));
            this.AddControl(PageType.Details, ControlCode.CreationDate, Field<Activity>.Create(x => x.CreatedDate));
            this.AddControl(PageType.Details, ControlCode.Offers, Field<Activity>.Create(x => x.Offers));
            this.AddControl(PageType.Details, ControlCode.Viewings, Field<Activity>.Create(x => x.Viewings));
            this.AddControl(PageType.Details, ControlCode.Attachments, Field<Activity>.Create(x => x.Attachments));
        }

        private void DefineControlsForCreate()
        {
            this.AddControl(PageType.Create, ControlCode.ActivityType, Field<CreateActivityCommand>.Create(x => x.ActivityTypeId).Required());
            this.AddControl(PageType.Create, ControlCode.ActivityStatus, Field<CreateActivityCommand>.Create(x => x.ActivityStatusId).Required());
            this.AddControl(PageType.Create, ControlCode.Departments, Field<CreateActivityCommand>.Create(x => x.Departments).Required().ExternalCollectionValidator(new UpdateActivityDepartmentValidator()));
            this.AddControl(PageType.Create, ControlCode.AskingPrice, Field<CreateActivityCommand>.Create(x => x.AskingPrice));
            this.AddControl(PageType.Create, ControlCode.ShortLetPricePerWeek, Field<CreateActivityCommand>.Create(x => x.ShortLetPricePerWeek));
            this.AddControl(PageType.Create, ControlCode.Vendors, Field<CreateActivityCommand>.Create(x => x.ContactIds));
            this.AddControl(PageType.Create, ControlCode.Landlords, Field<CreateActivityCommand>.Create(x => x.ContactIds));
            this.AddControl(PageType.Create, ControlCode.Negotiators,
                new List<IField>
                {
                    Field<CreateActivityCommand>.Create(x => x.LeadNegotiator).Required().ExternalValidator(new UpdateActivityUserValidator(true)),
                    Field<CreateActivityCommand>.Create(x => x.SecondaryNegotiators).ExternalCollectionValidator(new UpdateActivityUserValidator(false))
                });
        }

        private void DefineControlsForEdit()
        {
            this.AddControl(PageType.Update, ControlCode.ActivityType, Field<UpdateActivityCommand>.Create(x => x.ActivityTypeId).Required());
            this.AddControl(PageType.Update, ControlCode.ActivityStatus, Field<UpdateActivityCommand>.Create(x => x.ActivityStatusId).Required());
            this.AddControl(PageType.Update, ControlCode.Departments, Field<UpdateActivityCommand>.Create(x => x.Departments).Required().ExternalCollectionValidator(new UpdateActivityDepartmentValidator()));
            this.AddControl(PageType.Update, ControlCode.AskingPrice, Field<UpdateActivityCommand>.Create(x => x.AskingPrice));
            this.AddControl(PageType.Update, ControlCode.ShortLetPricePerWeek, Field<UpdateActivityCommand>.Create(x => x.ShortLetPricePerWeek));
            this.AddControl(PageType.Update, ControlCode.Vendors, Field<UpdateActivityCommand>.Create(x => x.ContactIds));
            this.AddControl(PageType.Update, ControlCode.Landlords, Field<UpdateActivityCommand>.Create(x => x.ContactIds));
            this.AddControl(PageType.Update, ControlCode.Negotiators,
                new List<IField>
                {
                    Field<UpdateActivityCommand>.Create(x => x.LeadNegotiator).Required().ExternalValidator(new UpdateActivityUserValidator(true)),
                    Field<UpdateActivityCommand>.Create(x => x.SecondaryNegotiators).ExternalCollectionValidator(new UpdateActivityUserValidator(false))
                });
        }

        public override void DefineMappings()
        {
            var openMarketLettingProperties = new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.GarageOnly, PropertyType.ParkingSpace,
                PropertyType.Houseboat
            };

            var freeholdSaleProperties = new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.FarmEstate,
                PropertyType.GarageOnly,PropertyType.ParkingSpace, PropertyType.Land, PropertyType.Houseboat
            };

            var longLeaseholdSaleProperties = new List<PropertyType>
            {
                PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land
            };
            
            this.Use(new List<ControlCode> { ControlCode.ActivityStatus, ControlCode.ActivityType }, this.ForAll(PageType.Preview, PageType.Details, PageType.Create, PageType.Update));
            this.Use(new List<ControlCode> { ControlCode.Departments, ControlCode.Negotiators }, this.ForAll(PageType.Details, PageType.Create, PageType.Update));

            this.Use(new List<ControlCode> { ControlCode.Landlords }, 
                this.When(openMarketLettingProperties, ActivityType.OpenMarketLetting, PageType.Preview, PageType.Details, PageType.Create, PageType.Update));

            this.Use(new List<ControlCode> { ControlCode.Vendors }, 
                this.When(freeholdSaleProperties, ActivityType.FreeholdSale, PageType.Preview, PageType.Details, PageType.Create, PageType.Update));

            this.Use(new List<ControlCode> { ControlCode.Vendors }, 
                this.When(longLeaseholdSaleProperties, ActivityType.LongLeaseholdSale, PageType.Preview, PageType.Details, PageType.Create, PageType.Update));

            this.Use(ControlCode.CreationDate, this.ForAll(PageType.Preview, PageType.Details));

            this.Use(new List<ControlCode> { ControlCode.Offers, ControlCode.Viewings, ControlCode.Attachments, ControlCode.Property }, this.ForAll(PageType.Details));

            this.Use(new List<ControlCode> { ControlCode.ShortLetPricePerWeek }, 
                this.When(openMarketLettingProperties, ActivityType.OpenMarketLetting, PageType.Details));

            this.Use(new List<ControlCode> { ControlCode.AskingPrice }, 
                this.When(freeholdSaleProperties, ActivityType.FreeholdSale, PageType.Details));

            this.Use(new List<ControlCode> { ControlCode.AskingPrice }, 
                this.When(longLeaseholdSaleProperties, ActivityType.LongLeaseholdSale, PageType.Details));

            this.Use(new List<ControlCode> {  ControlCode.ShortLetPricePerWeek }, 
                this.When(openMarketLettingProperties, ActivityType.OpenMarketLetting, PageType.Create, PageType.Update));

            this.Use(new List<ControlCode> { ControlCode.AskingPrice }, 
                this.When(freeholdSaleProperties, ActivityType.FreeholdSale, PageType.Create, PageType.Update));

            this.Use(new List<ControlCode> { ControlCode.AskingPrice }, 
                this.When(longLeaseholdSaleProperties, ActivityType.LongLeaseholdSale, PageType.Create, PageType.Update));
        }
    }
}
