/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Activity {
        id: string = '';
        propertyId: string = '';
        activityStatusId: string = '';
        activityTypeId: string = '';
        activityType: Dto.IActivityType = null;
        contacts: Contact[] = [];
        attachments: Attachment[] = [];
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
                if (activity.attachments) {
                    this.attachments = activity.attachments.map((attachment: Dto.IAttachment) =>{ return new Attachment(attachment) });
                }
                else {
                    this.attachments = [];
                }
            }
        }
    }
}