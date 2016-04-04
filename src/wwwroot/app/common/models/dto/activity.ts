module Antares.Common.Models.Dto {
    export class Activity implements IActivity {
        id: string = '';
        propertyId: string = '';
        activityStatusId: string = '';
        contacts: Contact[] = [];
        createdDate: Date = null;

        constructor(activity?: IActivity) {
            if (activity) {
                angular.extend(this, activity);

                this.createdDate = Core.DateTimeUtils.convertDateToUtc(activity.createdDate);
                this.contacts = activity.contacts.map((contact: IContact) => { return new Dto.Contact(contact) });
            }
        }
    }
}