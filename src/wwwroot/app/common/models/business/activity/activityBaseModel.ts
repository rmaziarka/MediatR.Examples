/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;
    import Enums = Common.Models.Enums;
    declare var moment: any;

    export class ActivityBaseModel {
        id: string = null;
        propertyId: string = '';
        activityStatusId: string = '';
        activityTypeId: string = '';
        activityType: Dto.IActivityType = null;
        contacts: Business.Contact[] = [];
        attachments: Business.Attachment[] = [];
        property: Business.PreviewProperty = null;
        createdDate: Date = null;
        viewingsByDay: Business.ViewingGroup[];
        viewings: Business.Viewing[];
        leadNegotiator: Business.ActivityUser = null;
        secondaryNegotiator: Business.ActivityUser[] = [];
        activityUsers: Business.ActivityUser[] = [];
        activityDepartments: Business.ActivityDepartment[] = [];
        offers: Business.Offer[];
        askingPrice: number = null;
        shortLetPricePerWeek: number = null;
        sourceId: string = null;
        sourceDescription: string = '';
        sellingReasonId: string = null;
        pitchingThreats: string = '';
        appraisalMeetingAttendees: Dto.IActivityAttendee[] = [];
        appraisalMeeting: Business.ActivityAppraisalMeeting;
        accessDetails: Business.ActivityAccessDetails = null;
        kfValuationPrice: number = null;
        agreedInitialMarketingPrice: number = null;
        vendorValuationPrice: number = null;
        shortKfValuationPrice: number = null;
        shortAgreedInitialMarketingPrice: number = null;
        shortVendorValuationPrice: number = null;
        longKfValuationPrice: number = null;
        longAgreedInitialMarketingPrice: number = null;
        longVendorValuationPrice: number = null;
        disposalTypeId: string = '';
        decorationId: string = '';
        serviceChargeAmount: number = null;
        serviceChargeNote: string = '';
        groundRentAmount: number = null;
        groundRentNote: string = '';
        otherCondition: string = '';
        priceTypeId: string = '';
        activityPrice: number = null;
        matchFlexibilityId: string = '';
        matchFlexValue: number = null;
        matchFlexPercentage: number = null;
        rentPaymentPeriodId: string = '';
        shortAskingWeekRent: number = null;
        shortAskingMonthRent: number = null;
        longAskingWeekRent: number = null;
        longAskingMonthRent: number = null;
        shortMatchFlexibilityId: string = '';
        shortMatchFlexWeekValue: number = null;
        shortMatchFlexMonthValue: number = null;
        shortMatchFlexPercentage: number = null;
        longMatchFlexibilityId: string = '';
        longMatchFlexWeekValue: number = null;
        longMatchFlexMonthValue: number = null;
        longMatchFlexPercentage: number = null;
        solicitor: Dto.IContact = null;
        solicitorCompany: Dto.ICompany = null;
        appraisalMeetingStart: string = null;
        appraisalMeetingEnd: string = null;
        appraisalMeetingInvitationText: string = null;
        keyNumber: string = null;
        accessArrangements: string = null;
        marketingStrapline: string = null;
        marketingFullDescription: string = null;
        marketingLocationDescription: string = null;
        advertisingPublishToWeb: boolean = false;
        advertisingPortals: Dto.IPortal[] = [];
        advertisingNote: string = null;
        advertisingPrPermitted: boolean = false;
        advertisingPrContent: string = null;
        salesBoardTypeId: string = null;
        salesBoardStatusId: string = null;
        salesBoardUpToDate: boolean;
        salesBoardRemovalDate: Date;
        salesBoardSpecialInstructions: string = null;
        chainTransactions: Business.ChainTransaction[] = [];

        constructor(activity?: Dto.IActivity) {
            if (activity) {
                angular.extend(this, activity);
                this.createdDate = Core.DateTimeUtils.convertDateToUtc(activity.createdDate);
                if (activity.contacts) {
                    this.contacts = activity.contacts.map((contact: Dto.IContact) => { return new Business.Contact(contact) });
                }
                this.property = new Business.PreviewProperty(activity.property);

                var activityleadNegotiator = _.find(activity.activityUsers,
                    (user: Dto.IActivityUser) => user.userType.code === Enums.NegotiatorTypeEnum[Enums.NegotiatorTypeEnum.LeadNegotiator]);
                this.leadNegotiator = new Business.ActivityUser(activityleadNegotiator);

                this.secondaryNegotiator = _.filter(activity.activityUsers,
                    (user: Dto.IActivityUser) => user.userType.code === Enums.NegotiatorTypeEnum[Enums.NegotiatorTypeEnum.SecondaryNegotiator])
                    .map((user: Dto.IActivityUser) => new Business.ActivityUser(user));

                if (activity.activityDepartments) {
                    this.activityDepartments = activity.activityDepartments.map((department: Dto.IActivityDepartment) => { return new Business.ActivityDepartment(department) });
                }

                if (activity.attachments) {
                    this.attachments = activity.attachments.map((attachment: Dto.IAttachment) => { return new Business.Attachment(attachment) });
                }
                else {
                    this.attachments = [];
                }

                if (activity.viewings) {
                    this.viewings = activity.viewings.map((item) => new Business.Viewing(item));
                    this.groupViewings(this.viewings);
                }

                if (activity.offers) {
                    this.offers = activity.offers.map((item) => new Business.Offer(item));
                }

                if (activity.chainTransactions) {
                    this.chainTransactions = activity.chainTransactions.map((chainTransaction: Dto.IChainTransaction) => { return new Business.ChainTransaction(chainTransaction) });
                }
                else {
                    this.chainTransactions = [];
                }

                this.appraisalMeetingAttendees = activity.appraisalMeetingAttendees;
                this.appraisalMeeting = new Business.ActivityAppraisalMeeting(activity.appraisalMeetingStart, activity.appraisalMeetingEnd, activity.appraisalMeetingInvitationText);
                this.accessDetails = new Business.ActivityAccessDetails(activity.keyNumber, activity.accessArrangements);

                this.marketingStrapline = activity.marketingStrapline;
                this.marketingFullDescription = activity.marketingFullDescription;
                this.marketingLocationDescription = activity.marketingLocationDescription;
                this.advertisingPublishToWeb = activity.advertisingPublishToWeb;
                this.advertisingPortals = activity.advertisingPortals;
                this.advertisingNote = activity.advertisingNote;
                this.advertisingPrPermitted = activity.advertisingPrPermitted;
                this.advertisingPrContent = activity.advertisingPrContent;
                this.salesBoardTypeId = activity.salesBoardTypeId;
                this.salesBoardStatusId = activity.salesBoardStatusId;
                this.salesBoardUpToDate = activity.salesBoardUpToDate;
                this.salesBoardRemovalDate = moment(activity.salesBoardRemovalDate).toDate();
                this.salesBoardSpecialInstructions = activity.salesBoardSpecialInstructions;
            }

            this.secondaryNegotiator = this.secondaryNegotiator || [];
            this.appraisalMeetingAttendees = this.appraisalMeetingAttendees || [];
            this.leadNegotiator = this.leadNegotiator || new Business.ActivityUser();
            this.appraisalMeeting = this.appraisalMeeting || new Business.ActivityAppraisalMeeting();
            this.accessDetails = this.accessDetails || new Business.ActivityAccessDetails(null, null);
        }

        groupViewings(viewings: Business.Viewing[]) {
            this.viewingsByDay = [];
            var groupedViewings: _.Dictionary<Business.Viewing[]> = _.groupBy(viewings, (i: Business.Viewing) => { return i.day; });
            this.viewingsByDay = <Business.ViewingGroup[]>_.map(groupedViewings, (items: Business.Viewing[], key: string) => { return new Business.ViewingGroup(key, items); });
        }
    }
}