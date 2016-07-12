namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions.Fields;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;

    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;
    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class ActivityControlsConfiguration : ControlsConfigurationPerTwoTypes<PropertyType, ActivityType>
    {
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly Guid activityStatusToLetUnavailableId;

        private Guid GetEnumTypeItemId(EnumType enumType, Enum enumTypeCode)
        {
            string enumTypeCodeString = enumTypeCode.ToString(); // EF needs string and doesn't work with Enum
            return
                this.enumTypeItemRepository
                .FindBy(x => x.EnumType.Code == enumType.ToString() && x.Code == enumTypeCodeString)
                .Select(x => x.Id)
                .Single();
        }

        public ActivityControlsConfiguration(IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.Init();
            this.activityStatusToLetUnavailableId = this.GetEnumTypeItemId(EnumType.ActivityStatus, ActivityStatus.ToLetUnavailable);
        }

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
            this.AddControl(PageType.Details, ControlCode.KeyNumber, Field<Activity>.Create(x => x.KeyNumber));
            this.AddControl(PageType.Details, ControlCode.AccessArrangements, Field<Activity>.Create(x => x.AccessArrangements));
            this.AddControl(PageType.Details, ControlCode.AppraisalMeeting,
                new List<IField>
                {
                    Field<Activity>.Create(x => x.AppraisalMeetingStart),
                    Field<Activity>.Create(x => x.AppraisalMeetingEnd),
                    Field<Activity>.Create(x => x.AppraisalMeetingAttendees),
                    Field<Activity>.Create(x => x.AppraisalMeetingInvitationText)
                });
            this.AddControl(PageType.Details, ControlCode.ServiceChargeAmount, Field<Activity>.Create(x => x.ServiceChargeAmount));
            this.AddControl(PageType.Details, ControlCode.ServiceChargeNote, Field<Activity>.Create(x => x.ServiceChargeNote));
            this.AddControl(PageType.Details, ControlCode.GroundRentAmount, Field<Activity>.Create(x => x.GroundRentAmount));
            this.AddControl(PageType.Details, ControlCode.GroundRentNote, Field<Activity>.Create(x => x.GroundRentNote));
            this.AddControl(PageType.Details, ControlCode.OtherCondition, Field<Activity>.Create(x => x.OtherCondition));
            this.AddControl(PageType.Details, ControlCode.DisposalType, Field<Activity>.Create(x => x.DisposalTypeId, x => x.DisposalType));
            this.AddControl(PageType.Details, ControlCode.Decoration, Field<Activity>.Create(x => x.DecorationId, x => x.Decoration));
            this.AddControl(PageType.Details, ControlCode.KfValuationPrice, Field<Activity>.Create(x => x.KfValuationPrice));
            this.AddControl(PageType.Details, ControlCode.VendorValuationPrice, Field<Activity>.Create(x => x.VendorValuationPrice));
            this.AddControl(PageType.Details, ControlCode.AgreedInitialMarketingPrice, Field<Activity>.Create(x => x.AgreedInitialMarketingPrice));
            this.AddControl(PageType.Details, ControlCode.ShortKfValuationPrice, Field<Activity>.Create(x => x.ShortKfValuationPrice));
            this.AddControl(PageType.Details, ControlCode.ShortVendorValuationPrice, Field<Activity>.Create(x => x.ShortVendorValuationPrice));
            this.AddControl(PageType.Details, ControlCode.ShortAgreedInitialMarketingPrice, Field<Activity>.Create(x => x.ShortAgreedInitialMarketingPrice));
            this.AddControl(PageType.Details, ControlCode.LongKfValuationPrice, Field<Activity>.Create(x => x.LongKfValuationPrice));
            this.AddControl(PageType.Details, ControlCode.LongVendorValuationPrice, Field<Activity>.Create(x => x.LongVendorValuationPrice));
            this.AddControl(PageType.Details, ControlCode.LongAgreedInitialMarketingPrice, Field<Activity>.Create(x => x.LongAgreedInitialMarketingPrice));
            this.AddControl(PageType.Details, ControlCode.PriceType, Field<Activity>.Create(x => x.PriceTypeId, x => x.PriceType));
            this.AddControl(PageType.Details, ControlCode.ActivityPrice, Field<Activity>.Create(x => x.ActivityPrice));
            this.AddControl(PageType.Details, ControlCode.MatchFlexibility, Field<Activity>.Create(x => x.MatchFlexibilityId, x => x.MatchFlexibility));
            this.AddControl(PageType.Details, ControlCode.MatchFlexValue, Field<Activity>.Create(x => x.MatchFlexValue));
            this.AddControl(PageType.Details, ControlCode.MatchFlexPercentage, Field<Activity>.Create(x => x.MatchFlexPercentage));
            this.AddControl(PageType.Details, ControlCode.RentPaymentPeriod, Field<Activity>.Create(x => x.RentPaymentPeriodId, x => x.RentPaymentPeriod));
            this.AddControl(PageType.Details, ControlCode.ShortAskingWeekRent, Field<Activity>.Create(x => x.ShortAskingWeekRent));
            this.AddControl(PageType.Details, ControlCode.ShortAskingMonthRent, Field<Activity>.Create(x => x.ShortAskingMonthRent));
            this.AddControl(PageType.Details, ControlCode.LongAskingWeekRent, Field<Activity>.Create(x => x.LongAskingWeekRent));
            this.AddControl(PageType.Details, ControlCode.LongAskingMonthRent, Field<Activity>.Create(x => x.LongAskingMonthRent));
            this.AddControl(PageType.Details, ControlCode.ShortMatchFlexibility, Field<Activity>.Create(x => x.ShortMatchFlexibilityId, x => x.ShortMatchFlexibility));
            this.AddControl(PageType.Details, ControlCode.ShortMatchFlexWeekValue, Field<Activity>.Create(x => x.ShortMatchFlexWeekValue));
            this.AddControl(PageType.Details, ControlCode.ShortMatchFlexMonthValue, Field<Activity>.Create(x => x.ShortMatchFlexMonthValue));
            this.AddControl(PageType.Details, ControlCode.ShortMatchFlexPercentage, Field<Activity>.Create(x => x.ShortMatchFlexPercentage));
            this.AddControl(PageType.Details, ControlCode.LongMatchFlexibility, Field<Activity>.Create(x => x.LongMatchFlexibilityId, x => x.LongMatchFlexibility));
            this.AddControl(PageType.Details, ControlCode.LongMatchFlexWeekValue, Field<Activity>.Create(x => x.LongMatchFlexWeekValue));
            this.AddControl(PageType.Details, ControlCode.LongMatchFlexMonthValue, Field<Activity>.Create(x => x.LongMatchFlexMonthValue));
            this.AddControl(PageType.Details, ControlCode.LongMatchFlexPercentage, Field<Activity>.Create(x => x.LongMatchFlexPercentage));
        }

        private void DefineControlsForCreateAndEdit()
        {
            foreach (PageType pageType in new[] { PageType.Create, PageType.Update })
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
                this.AddControl(pageType, ControlCode.AppraisalMeetingDate,
                    new List<IField>
                    {
                        Field<ActivityCommandBase>.Create(x => x.AppraisalMeetingStart).Required(),
                        Field<ActivityCommandBase>.Create(x => x.AppraisalMeetingEnd).Required()
                    });
                this.AddControl(pageType, ControlCode.AppraisalMeetingAttendees, Field<ActivityCommandBase>.Create(x => x.AppraisalMeetingAttendeesList).ExternalCollectionValidator(new UpdateActivityAttendeeValidator()));
                this.AddControl(pageType, ControlCode.AppraisalMeetingInvitation, Field<ActivityCommandBase>.Create(x => x.AppraisalMeetingInvitationText));
                this.AddControl(pageType, ControlCode.ServiceChargeAmount, Field<ActivityCommandBase>.Create(x => x.ServiceChargeAmount));
                this.AddControl(pageType, ControlCode.ServiceChargeNote, Field<ActivityCommandBase>.Create(x => x.ServiceChargeNote));
                this.AddControl(pageType, ControlCode.GroundRentAmount, Field<ActivityCommandBase>.Create(x => x.GroundRentAmount));
                this.AddControl(pageType, ControlCode.GroundRentNote, Field<ActivityCommandBase>.Create(x => x.GroundRentNote));
                this.AddControl(pageType, ControlCode.OtherCondition, Field<ActivityCommandBase>.Create(x => x.OtherCondition));
                this.AddControl(pageType, ControlCode.DisposalType, Field<ActivityCommandBase>.Create(x => x.DisposalTypeId).Required());
                this.AddControl(pageType, ControlCode.Decoration, Field<ActivityCommandBase>.Create(x => x.DecorationId));
                this.AddControl(pageType, ControlCode.KfValuationPrice, Field<ActivityCommandBase>.Create(x => x.KfValuationPrice).Required());
                this.AddControl(pageType, ControlCode.VendorValuationPrice, Field<ActivityCommandBase>.Create(x => x.VendorValuationPrice));
                this.AddControl(pageType, ControlCode.AgreedInitialMarketingPrice, Field<ActivityCommandBase>.Create(x => x.AgreedInitialMarketingPrice));
                this.AddControl(pageType, ControlCode.ShortKfValuationPrice, Field<ActivityCommandBase>.Create(x => x.ShortKfValuationPrice).Required());
                this.AddControl(pageType, ControlCode.ShortVendorValuationPrice, Field<ActivityCommandBase>.Create(x => x.ShortVendorValuationPrice));
                this.AddControl(pageType, ControlCode.ShortAgreedInitialMarketingPrice, Field<ActivityCommandBase>.Create(x => x.ShortAgreedInitialMarketingPrice));
                this.AddControl(pageType, ControlCode.LongKfValuationPrice, Field<ActivityCommandBase>.Create(x => x.LongKfValuationPrice).Required());
                this.AddControl(pageType, ControlCode.LongVendorValuationPrice, Field<ActivityCommandBase>.Create(x => x.LongVendorValuationPrice));
                this.AddControl(pageType, ControlCode.LongAgreedInitialMarketingPrice, Field<ActivityCommandBase>.Create(x => x.LongAgreedInitialMarketingPrice));
                this.AddControl(pageType, ControlCode.PriceType, Field<ActivityCommandBase>.Create(x => x.PriceTypeId));
                this.AddControl(pageType, ControlCode.ActivityPrice, Field<ActivityCommandBase>.Create(x => x.ActivityPrice).Required());
                this.AddControl(pageType, ControlCode.MatchFlexibility, Field<ActivityCommandBase>.Create(x => x.MatchFlexibilityId));
                this.AddControl(pageType, ControlCode.MatchFlexValue, Field<ActivityCommandBase>.Create(x => x.MatchFlexValue));
                this.AddControl(pageType, ControlCode.MatchFlexPercentage, Field<ActivityCommandBase>.Create(x => x.MatchFlexPercentage));
                this.AddControl(pageType, ControlCode.RentPaymentPeriod, Field<ActivityCommandBase>.Create(x => x.RentPaymentPeriodId));
                this.AddControl(pageType, ControlCode.ShortAskingWeekRent, Field<ActivityCommandBase>.Create(x => x.ShortAskingWeekRent));
                this.AddControl(pageType, ControlCode.ShortAskingMonthRent, Field<ActivityCommandBase>.Create(x => x.ShortAskingMonthRent));
                this.AddControl(pageType, ControlCode.LongAskingWeekRent, Field<ActivityCommandBase>.Create(x => x.LongAskingWeekRent));
                this.AddControl(pageType, ControlCode.LongAskingMonthRent, Field<ActivityCommandBase>.Create(x => x.LongAskingMonthRent));
                this.AddControl(pageType, ControlCode.ShortMatchFlexibility, Field<ActivityCommandBase>.Create(x => x.ShortMatchFlexibilityId));
                this.AddControl(pageType, ControlCode.ShortMatchFlexWeekValue, Field<ActivityCommandBase>.Create(x => x.ShortMatchFlexWeekValue));
                this.AddControl(pageType, ControlCode.ShortMatchFlexMonthValue, Field<ActivityCommandBase>.Create(x => x.ShortMatchFlexMonthValue));
                this.AddControl(pageType, ControlCode.ShortMatchFlexPercentage, Field<ActivityCommandBase>.Create(x => x.ShortMatchFlexPercentage));
                this.AddControl(pageType, ControlCode.LongMatchFlexibility, Field<ActivityCommandBase>.Create(x => x.LongMatchFlexibilityId));
                this.AddControl(pageType, ControlCode.LongMatchFlexWeekValue, Field<ActivityCommandBase>.Create(x => x.LongMatchFlexWeekValue));
                this.AddControl(pageType, ControlCode.LongMatchFlexMonthValue, Field<ActivityCommandBase>.Create(x => x.LongMatchFlexMonthValue));
                this.AddControl(pageType, ControlCode.LongMatchFlexPercentage, Field<ActivityCommandBase>.Create(x => x.LongMatchFlexPercentage));
            }
        }

        public override void DefineMappings()
        {
            Guid activityStatusMarketAppraisalId = this.GetEnumTypeItemId(EnumType.ActivityStatus, ActivityStatus.MarketAppraisal);

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

            this.Use(ControlCode.AppraisalMeeting, this.When(allResidentials, PageType.Details));

            this.Use(ControlCode.ShortLetPricePerWeek,
                this.When(openMarketLetting, PageType.Details, PageType.Create, PageType.Update));

            this.Use(new[] { ControlCode.AskingPrice, ControlCode.SellingReason },
                this.When(residentialSale, PageType.Details, PageType.Create, PageType.Update));

            this.Use(
                new[]
                {
                    ControlCode.Source, ControlCode.SourceDescription, ControlCode.PitchingThreats, ControlCode.KeyNumber, ControlCode.AccessArrangements
                },
                this.When(allResidentials, PageType.Details, PageType.Create, PageType.Update));

            this.Use(
                new[]
                {
                    ControlCode.AppraisalMeetingDate, ControlCode.AppraisalMeetingInvitation, ControlCode.AppraisalMeetingAttendees
                },
                this.When(allResidentials, PageType.Create, PageType.Update));

            this.Use(new List<ControlCode> { ControlCode.DisposalType },
                this.When(residentialSale, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusMarketAppraisalId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusMarketAppraisalId);

            this.Use(new List<ControlCode> { ControlCode.DisposalType },
               this.When(residentialSale, PageType.Details))
               .HiddenWhen<Activity>(x => x.ActivityStatusId != activityStatusMarketAppraisalId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.ServiceChargeAmount,
                    ControlCode.ServiceChargeNote,
                    ControlCode.GroundRentAmount,
                    ControlCode.GroundRentNote
                },
                this.When(longLeaseholdSale, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusMarketAppraisalId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusMarketAppraisalId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.ServiceChargeAmount,
                    ControlCode.ServiceChargeNote,
                    ControlCode.GroundRentAmount,
                    ControlCode.GroundRentNote
                },
                this.When(longLeaseholdSale, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != activityStatusMarketAppraisalId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.OtherCondition,
                    ControlCode.Decoration
                },
                this.When(allResidentials, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusMarketAppraisalId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusMarketAppraisalId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.OtherCondition,
                    ControlCode.Decoration
                },
                this.When(allResidentials, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != activityStatusMarketAppraisalId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.KfValuationPrice,
                    ControlCode.VendorValuationPrice,
                    ControlCode.AgreedInitialMarketingPrice
                },
                this.When(residentialSale, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusMarketAppraisalId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusMarketAppraisalId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.KfValuationPrice,
                    ControlCode.VendorValuationPrice,
                    ControlCode.AgreedInitialMarketingPrice
                },
                this.When(residentialSale, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != activityStatusMarketAppraisalId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.ShortKfValuationPrice,
                    ControlCode.ShortVendorValuationPrice,
                    ControlCode.ShortAgreedInitialMarketingPrice,
                    ControlCode.LongKfValuationPrice,
                    ControlCode.LongVendorValuationPrice,
                    ControlCode.LongAgreedInitialMarketingPrice
                },
                this.When(openMarketLetting, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusMarketAppraisalId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusMarketAppraisalId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.ShortKfValuationPrice,
                    ControlCode.ShortVendorValuationPrice,
                    ControlCode.ShortAgreedInitialMarketingPrice,
                    ControlCode.LongKfValuationPrice,
                    ControlCode.LongVendorValuationPrice,
                    ControlCode.LongAgreedInitialMarketingPrice
                },
                this.When(openMarketLetting, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != activityStatusMarketAppraisalId);
            
            this.DefineMappingsForPrices(openMarketLetting, residentialSale);
        }

        private void DefineMappingsForPrices(List<Tuple<PropertyType, ActivityType>> openMarketLetting, List<Tuple<PropertyType, ActivityType>> residentialSale)
        {
            Guid activityStatusForSaleUnavailableId = this.GetEnumTypeItemId(EnumType.ActivityStatus, ActivityStatus.ForSaleUnavailable);
            Guid matchFlexPriceValueId = this.GetEnumTypeItemId(EnumType.ActivityMatchFlexPrice, ActivityMatchFlexPrice.MinimumPrice);
            Guid matchFlexPricePercentageId = this.GetEnumTypeItemId(EnumType.ActivityMatchFlexPrice, ActivityMatchFlexPrice.Percentage);
            Guid matchFlexRentValueId = this.GetEnumTypeItemId(EnumType.ActivityMatchFlexRent, ActivityMatchFlexRent.MinimumRent);
            Guid matchFlexRentPercentageId = this.GetEnumTypeItemId(EnumType.ActivityMatchFlexRent, ActivityMatchFlexRent.Percentage);
            Guid rentPaymentPeriodWeekId = this.GetEnumTypeItemId(EnumType.RentPaymentPeriod, RentPaymentPeriod.Weekly);
            Guid rentPaymentPeriodMonthId = this.GetEnumTypeItemId(EnumType.RentPaymentPeriod, RentPaymentPeriod.Monthly);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.PriceType,
                    ControlCode.ActivityPrice,
                    ControlCode.MatchFlexibility
                },
                this.When(residentialSale, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusForSaleUnavailableId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusForSaleUnavailableId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.PriceType,
                    ControlCode.ActivityPrice,
                    ControlCode.MatchFlexibility
                },
                this.When(residentialSale, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != activityStatusForSaleUnavailableId);

            this.Use(
                new List<ControlCode> { ControlCode.MatchFlexValue },
                this.When(residentialSale, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusForSaleUnavailableId || x.MatchFlexibilityId != matchFlexPriceValueId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusForSaleUnavailableId || x.MatchFlexibilityId != matchFlexPriceValueId);

            this.Use(
                new List<ControlCode> { ControlCode.MatchFlexValue },
                this.When(residentialSale, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != activityStatusForSaleUnavailableId || x.MatchFlexibilityId != matchFlexPriceValueId);

            this.Use(
                new List<ControlCode> { ControlCode.MatchFlexPercentage },
                this.When(residentialSale, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusForSaleUnavailableId || x.MatchFlexibilityId != matchFlexPricePercentageId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != activityStatusForSaleUnavailableId || x.MatchFlexibilityId != matchFlexPricePercentageId);

            this.Use(
                new List<ControlCode> { ControlCode.MatchFlexPercentage },
                this.When(residentialSale, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != activityStatusForSaleUnavailableId || x.MatchFlexibilityId != matchFlexPricePercentageId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.RentPaymentPeriod,
                    ControlCode.ShortMatchFlexibility,
                    ControlCode.LongMatchFlexibility
                },
                this.When(openMarketLetting, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.RentPaymentPeriod,
                    ControlCode.ShortMatchFlexibility,
                    ControlCode.LongMatchFlexibility
                },
                this.When(openMarketLetting, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.ShortAskingWeekRent,
                    ControlCode.LongAskingWeekRent,
                },
                this.When(openMarketLetting, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.RentPaymentPeriodId != rentPaymentPeriodWeekId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.ShortAskingWeekRent,
                    ControlCode.LongAskingWeekRent,
                },
                this.When(openMarketLetting, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.ShortAskingMonthRent,
                    ControlCode.LongAskingMonthRent
                },
                this.When(openMarketLetting, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.RentPaymentPeriodId != rentPaymentPeriodMonthId);

            this.Use(
                new List<ControlCode>
                {
                    ControlCode.ShortAskingMonthRent,
                    ControlCode.LongAskingMonthRent
                },
                this.When(openMarketLetting, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId);

            this.Use(
                new List<ControlCode> { ControlCode.ShortMatchFlexWeekValue },
                this.When(openMarketLetting, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.ShortMatchFlexibilityId != matchFlexRentValueId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.ShortMatchFlexibilityId != matchFlexRentValueId || x.RentPaymentPeriodId != rentPaymentPeriodWeekId);

            this.Use(
                new List<ControlCode> { ControlCode.ShortMatchFlexWeekValue },
                this.When(openMarketLetting, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.ShortMatchFlexibilityId != matchFlexRentValueId);

            this.Use(
                new List<ControlCode> { ControlCode.ShortMatchFlexMonthValue },
                this.When(openMarketLetting, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.ShortMatchFlexibilityId != matchFlexRentValueId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.ShortMatchFlexibilityId != matchFlexRentValueId || x.RentPaymentPeriodId != rentPaymentPeriodMonthId);

            this.Use(
                new List<ControlCode> { ControlCode.ShortMatchFlexMonthValue },
                this.When(openMarketLetting, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.ShortMatchFlexibilityId != matchFlexRentValueId);

            this.Use(
                new List<ControlCode> { ControlCode.ShortMatchFlexPercentage },
                this.When(openMarketLetting, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.ShortMatchFlexibilityId != matchFlexRentPercentageId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.ShortMatchFlexibilityId != matchFlexRentPercentageId);

            this.Use(
                new List<ControlCode> { ControlCode.ShortMatchFlexPercentage },
                this.When(openMarketLetting, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.ShortMatchFlexibilityId != matchFlexRentPercentageId);

            this.Use(
                new List<ControlCode> { ControlCode.LongMatchFlexWeekValue },
                this.When(openMarketLetting, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.LongMatchFlexibilityId != matchFlexRentValueId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.LongMatchFlexibilityId != matchFlexRentValueId || x.RentPaymentPeriodId != rentPaymentPeriodWeekId);

            this.Use(
                new List<ControlCode> { ControlCode.LongMatchFlexWeekValue },
                this.When(openMarketLetting, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.LongMatchFlexibilityId != matchFlexRentValueId);

            this.Use(
                new List<ControlCode> { ControlCode.LongMatchFlexMonthValue },
                this.When(openMarketLetting, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.LongMatchFlexibilityId != matchFlexRentValueId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.LongMatchFlexibilityId != matchFlexRentValueId || x.RentPaymentPeriodId != rentPaymentPeriodMonthId);

            this.Use(
                new List<ControlCode> { ControlCode.LongMatchFlexMonthValue },
                this.When(openMarketLetting, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.LongMatchFlexibilityId != matchFlexRentValueId);

            this.Use(
                new List<ControlCode> { ControlCode.LongMatchFlexPercentage },
                this.When(openMarketLetting, PageType.Create, PageType.Update))
                .ReadonlyWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.LongMatchFlexibilityId != matchFlexRentPercentageId)
                .HiddenWhen<ActivityCommandBase>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.LongMatchFlexibilityId != matchFlexRentPercentageId);

            this.Use(
                new List<ControlCode> { ControlCode.LongMatchFlexPercentage },
                this.When(openMarketLetting, PageType.Details))
                .HiddenWhen<Activity>(x => x.ActivityStatusId != this.activityStatusToLetUnavailableId || x.LongMatchFlexibilityId != matchFlexRentPercentageId);
        }
    }
}
