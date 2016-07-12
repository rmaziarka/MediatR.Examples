declare module Antares.Common.Models.Dto {
    interface ICompany {
        id: string;
        name: string;
        websiteUrl: string;
        clientCarePageUrl: string;   
        clientCareStatus: Dto.IEnumTypeItem;
        contacts: IContact[];   
        companiesContacts: ICompanyContact[];

        description: string;
        category: Dto.IEnumTypeItem;        //todo! check
        type: Dto.IEnumTypeItem;            //todo! check
        isValid: boolean;
        //relationshipManager: IContact;      //todo! check
    }
}