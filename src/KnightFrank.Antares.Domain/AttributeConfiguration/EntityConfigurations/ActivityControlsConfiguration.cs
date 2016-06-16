namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
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
            this.AddBaseControl(PageType.Create, ControlCode.Property, Field<CreateActivityCommand>.Create(x => x.PropertyId).Required().InnerField);
            this.AddBaseControl(PageType.Create, ControlCode.LeadNegotiator, Field<CreateActivityCommand>.Create(x => x.LeadNegotiatorId).Required().InnerField);

            this.AddControl(PageType.Create, ControlCode.ActivityStatus, Field<CreateActivityCommand>.CreateDictionary(x => x.ActivityStatusId, nameof(ActivityStatus)).Required().InnerField);
            this.AddControl(PageType.Create, ControlCode.Vendors, Field<CreateActivityCommand>.Create(x => x.ContactIds).InnerField);
            this.AddControl(PageType.Create, ControlCode.Landlords, Field<CreateActivityCommand>.Create(x => x.ContactIds).InnerField);
            this.AddControl(PageType.Create, ControlCode.ActivityType, Field<CreateActivityCommand>.Create(x => x.ActivityTypeId).Required().InnerField);

            this.AddBaseControl(PageType.Preview, ControlCode.Property, Field<Activity>.Create(x => x.PropertyId).Required().InnerField);

            this.AddControl(PageType.Preview, ControlCode.ActivityStatus, Field<Activity>.CreateDictionary(x => x.ActivityStatusId, nameof(ActivityStatus)).Required().InnerField);
            this.AddControl(PageType.Preview, ControlCode.Vendors, Field<Activity>.Create(x => x.Contacts).InnerField);
            this.AddControl(PageType.Preview, ControlCode.Landlords, Field<Activity>.Create(x => x.Contacts).InnerField);
            this.AddControl(PageType.Preview, ControlCode.ActivityType, Field<Activity>.Create(x => x.ActivityTypeId).Required().InnerField);
        }

        private void DefineControlsForPreview()
        {
            this.AddControl(PageType.Preview, ControlCode.ActivityStatus, Field<Activity>.Create(x => x.ActivityStatusId).InnerField);
            this.AddControl(PageType.Preview, ControlCode.Vendors, Field<Activity>.Create(x => x.Contacts).InnerField);
            this.AddControl(PageType.Preview, ControlCode.Landlords, Field<Activity>.Create(x => x.Contacts).InnerField);
            this.AddControl(PageType.Preview, ControlCode.ActivityType, Field<Activity>.Create(x => x.ActivityTypeId).InnerField);
            this.AddControl(PageType.Preview, ControlCode.CreationDate, Field<Activity>.Create(x => x.CreatedDate).InnerField);
        }

        private void DefineControlsForDetailsView()
        {
            this.AddControl(PageType.Details, ControlCode.ActivityType, Field<Activity>.Create(x => x.ActivityTypeId).InnerField);
            this.AddControl(PageType.Details, ControlCode.ActivityStatus, Field<Activity>.Create(x => x.ActivityStatusId).InnerField);
            this.AddControl(PageType.Details, ControlCode.Vendors, Field<Activity>.Create(x => x.Contacts).InnerField);
            this.AddControl(PageType.Details, ControlCode.Property, Field<Activity>.Create(x => x.PropertyId).InnerField);
            this.AddControl(PageType.Details, ControlCode.LeadNegotiator, Field<Activity>.Create(x => x.ActivityUsers).InnerField);
            this.AddControl(PageType.Details, ControlCode.Landlords, Field<Activity>.Create(x => x.Contacts).InnerField);
            // next call date?
            this.AddControl(PageType.Details, ControlCode.SecondaryNegotiators, Field<Activity>.Create(x => x.ActivityUsers).InnerField);
            this.AddControl(PageType.Details, ControlCode.ManagingDepartment, Field<Activity>.Create(x => x.ActivityDepartments).InnerField);
            this.AddControl(PageType.Details, ControlCode.SecondaryDepartments, Field<Activity>.Create(x => x.ActivityDepartments).InnerField);
            this.AddControl(PageType.Details, ControlCode.AskingPrice, Field<Activity>.Create(x => x.AskingPrice).InnerField);
            this.AddControl(PageType.Details, ControlCode.ShortLetPricePerWeek, Field<Activity>.Create(x => x.ShortLetPricePerWeek).InnerField);
            this.AddControl(PageType.Details, ControlCode.CreationDate, Field<Activity>.Create(x => x.CreatedDate).InnerField);
            this.AddControl(PageType.Details, ControlCode.MarketAppraisalPrice, Field<Activity>.Create(x => x.MarketAppraisalPrice).InnerField);
            this.AddControl(PageType.Details, ControlCode.RecommendedPrice, Field<Activity>.Create(x => x.RecommendedPrice).InnerField);
            this.AddControl(PageType.Details, ControlCode.VendorEstimatedPrice, Field<Activity>.Create(x => x.VendorEstimatedPrice).InnerField);
            this.AddControl(PageType.Details, ControlCode.Offers, Field<Activity>.Create(x => x.Offers).InnerField);
            this.AddControl(PageType.Details, ControlCode.Viewings, Field<Activity>.Create(x => x.Viewings).InnerField);
            this.AddControl(PageType.Details, ControlCode.Attachments, Field<Activity>.Create(x => x.Attachments).InnerField);
        }

        private void DefineControlsForEdit()
        {
            this.AddControl(PageType.Update, ControlCode.ActivityStatus, Field<UpdateActivityCommand>.Create(x => x.ActivityStatusId).Required().InnerField);
            this.AddControl(PageType.Update, ControlCode.LeadNegotiator, Field<UpdateActivityCommand>.Create(x => x.LeadNegotiator).Required().InnerField);
            // next call date?
            this.AddControl(PageType.Update, ControlCode.SecondaryNegotiators, Field<UpdateActivityCommand>.Create(x => x.SecondaryNegotiators).InnerField);
            this.AddControl(PageType.Update, ControlCode.ManagingDepartment, Field<UpdateActivityCommand>.Create(x => x.Departments).InnerField);
            this.AddControl(PageType.Update, ControlCode.SecondaryDepartments, Field<UpdateActivityCommand>.Create(x => x.Departments).InnerField);
            this.AddControl(PageType.Update, ControlCode.AskingPrice, Field<UpdateActivityCommand>.Create(x => x.AskingPrice).InnerField);
            this.AddControl(PageType.Update, ControlCode.ShortLetPricePerWeek, Field<UpdateActivityCommand>.Create(x => x.ShortLetPricePerWeek).InnerField);
            this.AddControl(PageType.Update, ControlCode.MarketAppraisalPrice, Field<UpdateActivityCommand>.Create(x => x.MarketAppraisalPrice).InnerField);
            this.AddControl(PageType.Update, ControlCode.RecommendedPrice, Field<UpdateActivityCommand>.Create(x => x.RecommendedPrice).InnerField);
            this.AddControl(PageType.Update, ControlCode.VendorEstimatedPrice, Field<UpdateActivityCommand>.Create(x => x.VendorEstimatedPrice).InnerField);
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

            this.Use(new List<ControlCode> { ControlCode.Vendors, ControlCode.ActivityType }, this.When(new List<PropertyType>
            {
                PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land
            }, ActivityType.LongLeaseholdSale, PageType.Create, PageType.Preview));

            this.Use(ControlCode.ActivityStatus, this.When(new List<PropertyType>
            {
                PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land
            }, ActivityType.LongLeaseholdSale, PageType.Create, PageType.Preview))
                .FieldHasAllowed<CreateActivityCommand, Guid, ActivityStatus>(
                    x => x.ActivityStatusId,
                    new List<ActivityStatus>
                        {
                            ActivityStatus.MarketAppraisal,
                            ActivityStatus.PreAppraisal
                        });

            this.Use(ControlCode.CreationDate, this.ForAll(PageType.Preview, PageType.Details));

            this.Use(new List<ControlCode> {ControlCode.Offers, ControlCode.Viewings, ControlCode.Attachments, ControlCode.Property}, this.ForAll(PageType.Details));

            this.Use(new List<ControlCode>
            {
                ControlCode.Landlords,
                ControlCode.ActivityStatus,
                ControlCode.ActivityType,
                ControlCode.LeadNegotiator,
                ControlCode.SecondaryNegotiators,
                ControlCode.ManagingDepartment,
                ControlCode.SecondaryDepartments,
                //ControlCode.NextCallDate,
                ControlCode.ShortLetPricePerWeek,
            }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.GarageOnly, PropertyType.ParkingSpace, PropertyType.Houseboat
            }, ActivityType.OpenMarketLetting, PageType.Details));

            this.Use(new List<ControlCode>
            {
                ControlCode.Vendors,
                ControlCode.ActivityStatus,
                ControlCode.ActivityType,
                ControlCode.LeadNegotiator,
                ControlCode.SecondaryNegotiators,
                ControlCode.ManagingDepartment,
                ControlCode.SecondaryDepartments,
                //ControlCode.NextCallDate,
                ControlCode.AskingPrice
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
                ControlCode.LeadNegotiator,
                ControlCode.SecondaryNegotiators,
                ControlCode.ManagingDepartment,
                ControlCode.SecondaryDepartments,
                //ControlCode.NextCallDate,
                ControlCode.AskingPrice
            }, this.When(new List<PropertyType>
            {
                PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land
            }, ActivityType.LongLeaseholdSale, PageType.Details));

            this.Use(new List<ControlCode>
            {
                ControlCode.Landlords,
                ControlCode.ActivityStatus,
                ControlCode.LeadNegotiator,
                ControlCode.SecondaryNegotiators,
                ControlCode.ManagingDepartment,
                ControlCode.SecondaryDepartments,
                //ControlCode.NextCallDate,
                ControlCode.ShortLetPricePerWeek
            }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.GarageOnly, PropertyType.ParkingSpace, PropertyType.Houseboat
            }, ActivityType.OpenMarketLetting, PageType.Update));

            this.Use(new List<ControlCode>
            {
                ControlCode.Vendors,
                ControlCode.ActivityStatus,
                ControlCode.LeadNegotiator,
                ControlCode.SecondaryNegotiators,
                ControlCode.ManagingDepartment,
                ControlCode.SecondaryDepartments,
                //ControlCode.NextCallDate,
                ControlCode.AskingPrice
            }, this.When(new List<PropertyType>
            {
                PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.FarmEstate,
                PropertyType.GarageOnly, PropertyType.ParkingSpace, PropertyType.Land, PropertyType.Houseboat
            }, ActivityType.FreeholdSale, PageType.Update));

            this.Use(new List<ControlCode>
            {
                ControlCode.Vendors,
                ControlCode.ActivityStatus,
                ControlCode.LeadNegotiator,
                ControlCode.SecondaryNegotiators,
                ControlCode.ManagingDepartment,
                ControlCode.SecondaryDepartments,
                //ControlCode.NextCallDate,
                ControlCode.AskingPrice
            }, this.When(new List<PropertyType>
            {
                PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land
            }, ActivityType.LongLeaseholdSale, PageType.Update));
        }
    }
}
