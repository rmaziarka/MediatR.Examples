/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    import Dto = Antares.Common.Models.Dto;

    export class UpdateActivityAttendee implements Dto.IActivityAttendee {
        isSelected: boolean = null;
        user: User = null;
        userId: string = '';
        contact: Contact = null;
        contactId: string = '';

        constructor(user?: Dto.IUser, contact?: Dto.IContact, isSelected: boolean = false) {
            if (user) {
                this.user = new User(user);
                this.userId = user.id;
            }

            if (contact) {
                this.contact = new Contact(contact);
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