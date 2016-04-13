/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateActivityResource implements Dto.ICreateActivityResource {
        propertyId: string = '';
        activityStatusId: string = '';
        contactIds: string[] = [];

        constructor(activity?: Dto.IActivity) {
            if (activity) {
                this.propertyId = activity.propertyId;
                this.activityStatusId = activity.activityStatusId;
                this.contactIds = activity.contacts.map((contact: Business.Contact) => contact.id);
            }
        }
    }
}