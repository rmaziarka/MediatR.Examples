/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class TenancyContact {
        id: string = null;
        contactId: string = null;
        contact: Contact = null;
        contactType: EnumTypeItem = null;
        tenancyId: string = null;
    }
}