/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Activity {
        id: string = '';
        propertyId: string = '';
        activityStatusId: string = '';
        activityTypeId: string = '';
        activityType: Dto.IActivityType = null;
        contacts: Contact[] = [];
        attachments: Antares.Common.Models.Business.Attachment[] = [];
        property: Property = null;
        createdDate: Date = null;
        marketAppraisalPrice: number = null;
        recommendedPrice: number = null;
        vendorEstimatedPrice: number = null;
        viewingsByDay: ViewingGroup[];
        viewings: Viewing[];

        constructor(activity?: Dto.IActivity) {
            if (activity) {
                angular.extend(this, activity);
                this.createdDate = Core.DateTimeUtils.convertDateToUtc(activity.createdDate);
                this.contacts = activity.contacts.map((contact: Dto.IContact) => { return new Contact(contact) });
                this.property = new Property(activity.property);
                if (activity.attachments) {
                    this.attachments = activity.attachments.map((attachment: Dto.IAttachment) =>{ return new Antares.Common.Models.Business.Attachment(attachment) });
                }
                else {
                    this.attachments = [];
                }
                
                if (activity.viewings) {
                    this.viewings = activity.viewings.map((item) => new Viewing(item));
                    this.groupViewings(this.viewings);
                }
            }
        }

        groupViewings(viewings: Viewing[]) {
            this.viewingsByDay = [];
            var groupedViewings: _.Dictionary<Viewing[]> = _.groupBy(viewings, (i: Viewing) => { return i.day; });
            this.viewingsByDay = _.map(groupedViewings, (items: Viewing[], key: string) => { return new ViewingGroup(key, items); });
        }
    }
}