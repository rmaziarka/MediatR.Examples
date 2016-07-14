/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ContactUser {
        id: string ="";
        contactId: string = "";
        userId: string = "";
        user: User = null;
        userType: Dto.IEnumTypeItem;
        callDate: Date = null;

        constructor(contactUser?: Dto.IContactUser) {
            if (contactUser) {
                angular.extend(this, contactUser);
                this.user = new User(contactUser.user);
            }
        }
    }
}