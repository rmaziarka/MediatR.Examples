/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    import Dto = Models.Dto;
    import Enums = Antares.Common.Models.Enums;

    export class TenancyEditModel {
        id: string = null;
        activity: Business.ActivityPreviewModel;
        requirement: Business.RequirementPreviewModel;
        landlords: Business.Contact[];
        tenants: Business.Contact[];
        startDate: Date  = null;
        endDate: Date  = null;
        agreedRent: number; 

        constructor(dto?: Antares.Common.Models.Dto.ITenancy) {
            if (dto) {
                this.id = dto.id;
                this.activity = new Business.ActivityPreviewModel(dto.activity);
                this.requirement = new Business.RequirementPreviewModel(dto.requirement);

                this.landlords = this.getContacts(dto.contacts, Enums.TenancyContactType[Enums.TenancyContactType.Landlord]);
                this.tenants = this.getContacts(dto.contacts, Enums.TenancyContactType[Enums.TenancyContactType.Tenant]);

                this.startDate = Core.DateTimeUtils.convertDateToUtc(dto.terms[0].startDate);
                this.endDate = Core.DateTimeUtils.convertDateToUtc(dto.terms[0].endDate);
                this.agreedRent = dto.terms[0].agreedRent;

            }
        }

        getContacts = (contacts: Dto.ITenancyContact[], contactType: string): Business.Contact[] => {
            return contacts
                .filter((contact: Dto.ITenancyContact) => { return contact.contactType.code == contactType })
                .map((tenancyContact: Dto.ITenancyContact) => { return new Business.Contact(tenancyContact.contact) });
        }
    }
}