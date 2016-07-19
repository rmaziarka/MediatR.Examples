declare module Antares.Common.Models.Dto {
    interface ITenancy {
        id: string;
        requirementId: string;
        requirement: IRequirement;
        activityId: string;
        activity: IActivity;
        contacts: ITenancyContact[];
        terms: ITenancyTerm[];
        tenancyTypeId: string;
        tenancyType: IEnumTypeItem;
    }
}