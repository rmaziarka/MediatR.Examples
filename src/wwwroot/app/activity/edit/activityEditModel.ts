/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity  {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ActivityEditModel {
        id: string = null;
        propertyId: string = '';
        activityStatusId: string = '';
        activityTypeId: string = '';
        activityType: Dto.IActivityType = null;
        contacts: Business.Contact[] = [];
        attachments: Business.Attachment[] = [];
        property: Business.PreviewProperty = null;
        createdDate: Date = null;
        marketAppraisalPrice: number = null;
        recommendedPrice: number = null;
        vendorEstimatedPrice: number = null;
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

                this.appraisalMeetingAttendees = activity.appraisalMeetingAttendees;
                this.appraisalMeeting = new Business.ActivityAppraisalMeeting(activity.appraisalMeetingStart, activity.appraisalMeetingEnd, activity.appraisalMeetingInvitationText);
                this.accessDetails = new Business.ActivityAccessDetails(activity.keyNumber, activity.accessArrangements);
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