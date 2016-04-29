declare module Antares.Common.Models.Dto {
    interface IViewing {
        id: string;
        activityId: string;
        requirementId: string;
        negotiatorId: string;
        startDate: Date;
        endDate: Date;
        invitationText: string;
        postviewingComment: string;
        attendeesIds: string[];
    }
}