/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.AddPanel {
    import Business = Antares.Common.Models.Business;

    export class ActivityAddPanelCommand {
        propertyId: string = '';
        activityStatusId: string = '';
        activityTypeId: string = '';
        contactIds: string[] = [];

        constructor(model: AddCard.ActivityAddCardModel, propertyId: string) {
            this.activityStatusId = model.activityStatusId;
            this.activityTypeId = model.activityTypeId;
            this.propertyId = propertyId;
            this.contactIds = model.contacts.map((contact: Business.Contact) => contact.id);
        }
    }

}