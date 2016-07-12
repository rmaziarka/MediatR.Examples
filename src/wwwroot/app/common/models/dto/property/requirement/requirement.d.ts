declare module Antares.Common.Models.Dto {
    interface IRequirement {
        [index: string]: any;

        id: string;
        requirementTypeId: string;
        requirementType: Dto.IResourceType;

        contacts: IContact[];
        address: IAddress;
        createDate: Date;
        requirementNotes: IRequirementNote[];

        rentMin?: number;
        rentMax?: number;

        description?: string;

        viewings?: IViewing[];
        offers?: IOffer[];

        attachments: IAttachment[];
        solicitor: IContact;
        solicitorCompany: ICompany;

        tenancy: ITenancy;
    }
}