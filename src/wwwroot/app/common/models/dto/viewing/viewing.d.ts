declare module Antares.Common.Models.Dto {
    interface IViewing {
        id: string;
        activityId: string;
        requirementId: string;
        negotiatorId: string;
        startDate: Date | string;
        endDate: Date | string;
        invitationText: string;
        postViewingComment: string;
        attendeesIds: string[];
        activity: Dto.IActivity;
        attendees: Dto.IContact[];
    }
}