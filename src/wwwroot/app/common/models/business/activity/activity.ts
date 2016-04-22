/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Activity {
        id: string = '';
        propertyId: string = '';
        activityStatusId: string = '';
        contacts: Contact[] = [];
        property: Property = null;
        createdDate: Date = null;
        marketAppraisalPrice: number = null;
        recommendedPrice: number = null;
        vendorEstimatedPrice: number = null;

        constructor(activity?: Dto.IActivity) {
            if (activity) {
                angular.extend(this, activity);

                this.createdDate = Core.DateTimeUtils.convertDateToUtc(activity.createdDate);
                this.contacts = activity.contacts.map((contact: Dto.IContact) => { return new Contact(contact) });
                this.property = new Property(activity.property);
            }
        }
    }
}