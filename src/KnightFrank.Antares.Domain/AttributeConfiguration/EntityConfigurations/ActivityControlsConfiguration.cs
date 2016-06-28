namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions.Fields;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;

    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;

    public class ActivityControlsConfiguration : ControlsConfigurationPerTwoTypes<PropertyType, ActivityType>
    {
        public override void DefineControls()
        {
            this.DefineControlsForCreateAndEdit();
            this.DefineControlsForDetailsView();
            this.DefineControlsForPreview();
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
            this.AddControl(PageType.Details, ControlCode.Source, Field<Activity>.Create(x => x.SourceId, x => x.Source));
            this.AddControl(PageType.Details, ControlCode.SourceDescription, Field<Activity>.Create(x => x.SourceDescription));
            this.AddControl(PageType.Details, ControlCode.SellingReason, Field<Activity>.Create(x => x.SellingReasonId, x => x.SellingReason));
            this.AddControl(PageType.Details, ControlCode.PitchingThreats, Field<Activity>.Create(x => x.PitchingThreats));
        }

        private void DefineControlsForCreateAndEdit()
        {
            foreach(PageType pageType in new [] {PageType.Create, PageType.Update })
            {
                this.AddControl(pageType, ControlCode.ActivityType, Field<ActivityCommandBase>.Create(x => x.ActivityTypeId).Required());
                this.AddControl(pageType, ControlCode.ActivityStatus, Field<ActivityCommandBase>.Create(x => x.ActivityStatusId).Required());
                this.AddControl(pageType, ControlCode.Departments, Field<ActivityCommandBase>.Create(x => x.Departments).Required().ExternalCollectionValidator(new UpdateActivityDepartmentValidator()));
                this.AddControl(pageType, ControlCode.AskingPrice, Field<ActivityCommandBase>.Create(x => x.AskingPrice));
                this.AddControl(pageType, ControlCode.ShortLetPricePerWeek, Field<ActivityCommandBase>.Create(x => x.ShortLetPricePerWeek));
                this.AddControl(pageType, ControlCode.Vendors, Field<ActivityCommandBase>.Create(x => x.ContactIds));
                this.AddControl(pageType, ControlCode.Landlords, Field<ActivityCommandBase>.Create(x => x.ContactIds));
                this.AddControl(pageType, ControlCode.Negotiators,
                    new List<IField>
                    {
                        Field<ActivityCommandBase>.Create(x => x.LeadNegotiator).Required().ExternalValidator(new UpdateActivityUserValidator(true)),
                        Field<ActivityCommandBase>.Create(x => x.SecondaryNegotiators).ExternalCollectionValidator(new UpdateActivityUserValidator(false))
                    });
                this.AddControl(pageType, ControlCode.Source, Field<ActivityCommandBase>.Create(x => x.SourceId).Required());
                this.AddControl(pageType, ControlCode.SourceDescription, Field<ActivityCommandBase>.Create(x => x.SourceDescription));
                this.AddControl(pageType, ControlCode.SellingReason, Field<ActivityCommandBase>.Create(x => x.SellingReasonId).Required());
                this.AddControl(pageType, ControlCode.PitchingThreats, Field<ActivityCommandBase>.Create(x => x.PitchingThreats));
                this.AddControl(pageType, ControlCode.KeyNumber, Field<ActivityCommandBase>.Create(x => x.KeyNumber));
                this.AddControl(pageType, ControlCode.AccessArrangements, Field<ActivityCommandBase>.Create(x => x.AccessArrangements));
            }
        }

        public override void DefineMappings()
        {
            List<Tuple<PropertyType, ActivityType>> openMarketLetting =
                new[]
                {
                    PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette, PropertyType.GarageOnly,
                    PropertyType.ParkingSpace, PropertyType.Houseboat
                }.Select(p => Tuple.Create(p, ActivityType.OpenMarketLetting)).ToList();

            List<Tuple<PropertyType, ActivityType>> freeholdSale =
                new[]
                {
                    PropertyType.House, PropertyType.Flat, PropertyType.Bungalow, PropertyType.Maisonette,
                    PropertyType.DevelopmentPlot, PropertyType.FarmEstate,
                    PropertyType.GarageOnly, PropertyType.ParkingSpace, PropertyType.Land, PropertyType.Houseboat
                }.Select(p => Tuple.Create(p, ActivityType.FreeholdSale)).ToList();

            List<Tuple<PropertyType, ActivityType>> longLeaseholdSale =
                new[] { PropertyType.Flat, PropertyType.Maisonette, PropertyType.DevelopmentPlot, PropertyType.Land }
                    .Select(p => Tuple.Create(p, ActivityType.LongLeaseholdSale))
                    .ToList();

            List<Tuple<PropertyType, ActivityType>> residentialSale = freeholdSale.Union(longLeaseholdSale).ToList();
            List<Tuple<PropertyType, ActivityType>> allResidentials = residentialSale.Union(openMarketLetting).ToList();

            this.Use(ControlCode.CreationDate, this.ForAll(PageType.Preview, PageType.Details));

            this.Use(new[] { ControlCode.ActivityStatus, ControlCode.ActivityType },
                this.When(allResidentials, PageType.Preview, PageType.Details, PageType.Create, PageType.Update));

            this.Use(new[] { ControlCode.Departments, ControlCode.Negotiators },
                this.When(allResidentials, PageType.Details, PageType.Create, PageType.Update));

            this.Use(ControlCode.Landlords,
                this.When(openMarketLetting, PageType.Preview, PageType.Details, PageType.Create, PageType.Update));

            this.Use(ControlCode.Vendors,
                this.When(residentialSale, PageType.Preview, PageType.Details, PageType.Create, PageType.Update));

            this.Use(new[] { ControlCode.Offers, ControlCode.Viewings, ControlCode.Attachments, ControlCode.Property },
                this.ForAll(PageType.Details));

            this.Use(ControlCode.ShortLetPricePerWeek,
                this.When(openMarketLetting, PageType.Details, PageType.Create, PageType.Update));

            this.Use(new[] { ControlCode.AskingPrice, ControlCode.SellingReason },
                this.When(residentialSale, PageType.Details, PageType.Create, PageType.Update));

            this.Use(new[] { ControlCode.Source, ControlCode.SourceDescription, ControlCode.PitchingThreats },
                this.When(allResidentials, PageType.Details, PageType.Create, PageType.Update));

            this.Use(new[] { ControlCode.KeyNumber, ControlCode.AccessArrangements },
                this.When(allResidentials, PageType.Create, PageType.Update));
        }
    }
}
