/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateActivityResource implements Dto.ICreateActivityResource {
        propertyId: string = '';
        activityStatusId: string = '';
        activityTypeId: string = '';
        contactIds: string[] = [];

        constructor(activity?: Business.Activity) {
            if (activity) {
                this.propertyId = activity.propertyId;
                this.activityStatusId = activity.activityStatusId;
                this.activityTypeId = activity.activityTypeId;
                this.contactIds = activity.contacts.map((contact: Business.Contact) => contact.id);
            }
        }
    }
}