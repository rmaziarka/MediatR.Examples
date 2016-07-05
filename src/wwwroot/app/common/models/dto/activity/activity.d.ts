declare module Antares.Common.Models.Dto {
    interface IActivity {
        id: string;
        propertyId: string;
        activityStatusId: string;
        activityTypeId: string;
        contacts: IContact[];
        attachments?: IAttachment[];
        property?: IPreviewProperty;
        createdDate?: Date;
        marketAppraisalPrice?: number;
        recommendedPrice?: number;
        vendorEstimatedPrice?: number;
        viewings?: IViewing[];
        activityUsers: IActivityUser[];
        activityDepartments: IActivityDepartment[];
        offers?: IOffer[];
        askingPrice?: number;
        shortLetPricePerWeek?: number;
        solicitor: IContact;
        solicitorCompany: ICompany;
        sourceId: string;
        sellingReasonId: string;
        appraisalMeetingStart: string;
        appraisalMeetingEnd: string;
        appraisalMeetingInvitationText: string;
        keyNumber: string;
        accessArrangements: string;
        appraisalMeetingAttendees: Dto.IActivityAttendee[];
    }
}