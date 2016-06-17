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

    public class ActivityControlsConfiguration : BaseControlsConfiguration<PropertyType, ActivityType>
    {
        public override void DefineControls()
        {
            this.DefineControlsForCreate();
            this.DefineControlsForDetailsView();
            this.DefineControlsForPreview();
            this.DefineControlsForEdit();
        }

        private void DefineControlsForCreate()
        {
            this.AddControl(PageType.Create, ControlCode.ActivityStatus, Field<CreateActivityCommand>.CreateDictionary(x => x.ActivityStatusId, nameof(ActivityStatus)).Required());
            this.AddControl(PageType.Create, ControlCode.Vendors, Field<CreateActivityCommand>.Create(x => x.ContactIds));
            this.AddControl(PageType.Create, ControlCode.Landlords, Field<CreateActivityCommand>.Create(x => x.ContactIds));
            this.AddControl(PageType.Create, ControlCode.ActivityType, Field<CreateActivityCommand>.Create(x => x.ActivityTypeId).Required());
        }

        private void DefineControlsForPreview()
        {
            this.AddControl(PageType.Preview, ControlCode.ActivityStatus, Field<Activity>.Create(x => x.ActivityStatusId));
            this.AddControl(PageType.Preview, ControlCode.Vendors, Field<Activity>.Create(x => x.Contacts));
            this.AddControl(PageType.Preview, ControlCode.Landlords, Field<Activity>.Create(x => x.Contacts));
            this.AddControl(PageType.Preview, ControlCode.ActivityType, Field<Activity>.Create(x => x.ActivityTypeId));
            this.AddControl(PageType.Preview, ControlCode.CreationDate, Field<Activity>.Create(x => x.CreatedDate));
        }

        private void DefineControlsForDetailsView()
        {
            this.AddControl(PageType.Details, ControlCode.ActivityType, Field<Activity>.Create(x => x.ActivityTypeId));
            this.AddControl(PageType.Details, ControlCode.ActivityStatus, Field<Activity>.Create(x => x.ActivityStatusId));
            this.AddControl(PageType.Details, ControlCode.Vendors, Field<Activity>.Create(x => x.Contacts));
            this.AddControl(PageType.Details, ControlCode.Property, Field<Activity>.Create(x => x.PropertyId));
            this.AddControl(PageType.Details, ControlCode.Negotiators, Field<Activity>.Create(x => x.ActivityUsers));
            this.AddControl(PageType.Details, ControlCode.Landlords, Field<Activity>.Create(x => x.Contacts));
            this.AddControl(PageType.Details, ControlCode.Departments, Field<Activity>.Create(x => x.ActivityDepartments));
            this.AddControl(PageType.Details, ControlCode.AskingPrice, Field<Activity>.Create(x => x.AskingPrice));
            this.AddControl(PageType.Details, ControlCode.ShortLetPricePerWeek, Field<Activity>.Create(x => x.ShortLetPricePerWeek));
            this.AddControl(PageType.Details, ControlCode.CreationDate, Field<Activity>.Create(x => x.CreatedDate));
            this.AddControl(PageType.Details, ControlCode.MarketAppraisalPrice, Field<Activity>.Create(x => x.MarketAppraisalPrice));
            this.AddControl(PageType.Details, ControlCode.RecommendedPrice, Field<Activity>.Create(x => x.RecommendedPrice));
            this.AddControl(PageType.Details, ControlCode.VendorEstimatedPrice, Field<Activity>.Create(x => x.VendorEstimatedPrice));
            this.AddControl(PageType.Details, ControlCode.Offers, Field<Activity>.Create(x => x.Offers));
            this.AddControl(PageType.Details, ControlCode.Viewings, Field<Activity>.Create(x => x.Viewings));
            this.AddControl(PageType.Details, ControlCode.Attachments, Field<Activity>.Create(x => x.Attachments));
        }

        private void DefineControlsForEdit()
        {
            this.AddControl(PageType.Update, ControlCode.ActivityStatus, Field<UpdateActivityCommand>.Create(x => x.ActivityStatusId).Required());
            this.AddControl(PageType.Update, ControlCode.Departments, Field<UpdateActivityCommand>.Create(x => x.Departments).Required().ExternalCollectionValidator(new UpdateActivityDepartmentValidator()));
            this.AddControl(PageType.Update, ControlCode.AskingPrice, Field<UpdateActivityCommand>.Create(x => x.AskingPrice));
            this.AddControl(PageType.Update, ControlCode.ShortLetPricePerWeek, Field<UpdateActivityCommand>.Create(x => x.ShortLetPricePerWeek));
            this.AddControl(PageType.Update, ControlCode.MarketAppraisalPrice, Field<UpdateActivityCommand>.Create(x => x.MarketAppraisalPrice).GreaterThanOrEqualTo(0));
            this.AddControl(PageType.Update, ControlCode.RecommendedPrice, Field<UpdateActivityCommand>.Create(x => x.RecommendedPrice).GreaterThanOrEqualTo(0));
            this.AddControl(PageType.Update, ControlCode.VendorEstimatedPrice, Field<UpdateActivityCommand>.Create(x => x.VendorEstimatedPrice).GreaterThanOrEqualTo(0));
            this.AddControl(PageType.Update, ControlCode.Negotiators,
                new List<IField>
                {
                    Field<UpdateActivityCommand>.Create(x => x.LeadNegotiator).Required().ExternalValidator(new UpdateActivityUserValidator(true)),
                    Field<UpdateActivityCommand>.Create(x => x.SecondaryNegotiators).Required().ExternalCollectionValidator(new UpdateActivityUserValidator(false))
                });
        }

        public override void DefineMappings()
        {
            this.Use(new List<ControlCode> { ControlCode.ActivityStatus, ControlCode.Landlords, ControlCode.ActivityType }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.GarageOnly,PropertyType.ParkingSpace, PropertyType.Houseboat
            }, ActivityType.OpenMarketLetting, PageType.Create, PageType.Preview));

            this.Use(new List<ControlCode> { ControlCode.ActivityStatus, ControlCode.Vendors, ControlCode.ActivityType }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.FarmEstate,
                PropertyType.GarageOnly,PropertyType.ParkingSpace, PropertyType.Land, PropertyType.Houseboat
            }, ActivityType.FreeholdSale, PageType.Create, PageType.Preview));

            this.Use(new List<ControlCode> { ControlCode.ActivityStatus, ControlCode.Vendors, ControlCode.ActivityType }, this.When(new List<PropertyType>
            {
                PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land
            }, ActivityType.LongLeaseholdSale, PageType.Create, PageType.Preview));

            this.Use(ControlCode.CreationDate, this.ForAll(PageType.Preview, PageType.Details));

            this.Use(new List<ControlCode> { ControlCode.Offers, ControlCode.Viewings, ControlCode.Attachments, ControlCode.Property }, this.ForAll(PageType.Details));

            this.Use(new List<ControlCode>
            {
                ControlCode.Landlords,
                ControlCode.ActivityStatus,
                ControlCode.ActivityType,
                ControlCode.Negotiators,
                ControlCode.Departments,
                ControlCode.ShortLetPricePerWeek,
                ControlCode.MarketAppraisalPrice,
                ControlCode.RecommendedPrice,
                ControlCode.VendorEstimatedPrice
            }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.GarageOnly, PropertyType.ParkingSpace, PropertyType.Houseboat
            }, ActivityType.OpenMarketLetting, PageType.Details));

            this.Use(new List<ControlCode>
            {
                ControlCode.Vendors,
                ControlCode.ActivityStatus,
                ControlCode.ActivityType,
                ControlCode.Negotiators,
                ControlCode.Departments,
                ControlCode.AskingPrice,
                ControlCode.MarketAppraisalPrice,
                ControlCode.RecommendedPrice,
                ControlCode.VendorEstimatedPrice
            }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.FarmEstate,
                PropertyType.GarageOnly, PropertyType.ParkingSpace, PropertyType.Land, PropertyType.Houseboat
            }, ActivityType.FreeholdSale, PageType.Details));

            this.Use(new List<ControlCode>
            {
                ControlCode.Vendors,
                ControlCode.ActivityStatus,
                ControlCode.ActivityType,
                ControlCode.Negotiators,
                ControlCode.Departments,
                ControlCode.AskingPrice,
                ControlCode.MarketAppraisalPrice,
                ControlCode.RecommendedPrice,
                ControlCode.VendorEstimatedPrice
            }, this.When(new List<PropertyType>
            {
                PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land
            }, ActivityType.LongLeaseholdSale, PageType.Details));

            this.Use(new List<ControlCode>
            {
                ControlCode.ActivityStatus,
                ControlCode.Negotiators,
                ControlCode.Departments,
                ControlCode.ShortLetPricePerWeek,
                ControlCode.MarketAppraisalPrice,
                ControlCode.RecommendedPrice,
                ControlCode.VendorEstimatedPrice
            }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.GarageOnly, PropertyType.ParkingSpace, PropertyType.Houseboat
            }, ActivityType.OpenMarketLetting, PageType.Update));

            this.Use(new List<ControlCode>
            {
                ControlCode.ActivityStatus,
                ControlCode.Negotiators,
                ControlCode.Departments,
                ControlCode.AskingPrice,
                ControlCode.MarketAppraisalPrice,
                ControlCode.RecommendedPrice,
                ControlCode.VendorEstimatedPrice
            }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.FarmEstate,
                PropertyType.GarageOnly, PropertyType.ParkingSpace, PropertyType.Land, PropertyType.Houseboat
            }, ActivityType.FreeholdSale, PageType.Update));

            this.Use(new List<ControlCode>
            {
                ControlCode.ActivityStatus,
                ControlCode.Negotiators,
                ControlCode.Departments,
                ControlCode.AskingPrice,
                ControlCode.MarketAppraisalPrice,
                ControlCode.RecommendedPrice,
                ControlCode.VendorEstimatedPrice
            }, this.When(new List<PropertyType>
            {
                PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land
            }, ActivityType.LongLeaseholdSale, PageType.Update));
        }
    }
}
