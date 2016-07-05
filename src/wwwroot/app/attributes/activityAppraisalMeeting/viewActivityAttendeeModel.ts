/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes.Models {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    export class ViewActivityAttendeeModel {
        user: Business.User = null;
        userId: string = '';
        contact: Business.Contact = null;
        contactId: string = '';

        constructor(attendee?: Dto.IActivityAttendee) {
            if (attendee) {
                if (attendee.user) {
                    this.user = new Business.User(attendee.user);
                    this.userId = attendee.user.id;
                }

                if (attendee.contact) {
                    this.contact = new Business.Contact(attendee.contact);
                    this.contactId = attendee.contact.id;
                }
            }
        }

        public getName(): string {
            return this.user ? this.user.getName() : this.contact.getName();
        }
    }
}