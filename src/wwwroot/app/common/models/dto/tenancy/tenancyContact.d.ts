declare module Antares.Common.Models.Dto {
    interface ITenancyContact {
        id: string;
        contactId: string;
        contact: Dto.IContact;
        contactType: Dto.IEnumTypeItem;
        tenancyId: string;
    }
}