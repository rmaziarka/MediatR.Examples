/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes.Models {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    export class EditActivityAttendeeModel implements Dto.IActivityAttendee {
        isSelected: boolean = null;
        user: Business.User = null;
        userId: string = '';
        contact: Business.Contact = null;
        contactId: string = '';

        constructor(user?: Dto.IUser, contact?: Dto.IContact, isSelected: boolean = false) {
            if (user) {
                this.user = new Business.User(user);
                this.userId = user.id;
            }

            if (contact) {
                this.contact = new Business.Contact(contact);
                this.contactId = contact.id;
            }

            this.isSelected = isSelected;
        }

        public getName(): string {
            return this.user ? this.user.getName() : this.contact.getName();
        }

        public getId(): string {
            return this.user ? this.userId : this.contactId;
        }
    }
}