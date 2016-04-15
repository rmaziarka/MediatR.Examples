declare module Antares.Common.Models.Dto {
    interface IRequirementNote {
        id: string;
        requirementId: string;
        description: string;
        createdDate: Date;
        user: IUser;
    }
}