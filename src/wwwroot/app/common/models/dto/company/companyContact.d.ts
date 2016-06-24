declare module Antares.Common.Models.Dto {
    interface ICompanyContact {
        id: string;
        contactId: string;
        contact: IContact;
        companyId: string;
        company: ICompany;
    }
}