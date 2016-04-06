/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Activity implements Dto.IActivity {
        id: string = '';
        propertyId: string = '';
        activityStatusId: string = '';
        contacts: Dto.Contact[] = [];
        property: Dto.Property = null;
        createdDate: Date = null;

        constructor(activity?: Dto.IActivity) {
            if (activity) {
                angular.extend(this, activity);

                this.createdDate = Core.DateTimeUtils.convertDateToUtc(activity.createdDate);
                this.contacts = activity.contacts.map((contact: Dto.IContact) => { return new Dto.Contact(contact) });
            }
        }
    }
}