/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.AddCard {
    import Business = Antares.Common.Models.Business;

    export class ActivityAddCardModel {
        id: string = '';
        activityStatusId: string = '';
        activityTypeId: string = '';
        contacts: Business.Contact[] = [];
    }

}