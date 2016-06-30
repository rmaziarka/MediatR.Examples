declare module Antares.Common.Models.Dto {
    interface ICreateRequirementResource {
        requirementTypeId: string;
        contactIds: string[];
        address: IAddress;

        rentMin?: number;
        rentMax?: number;

        description?: string;
    }
}