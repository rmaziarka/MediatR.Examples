/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityAddCardController {
        // bindings
        propertyTypeId: string;
        ownerships: Business.Ownership[];
        onSave: (obj: { activity: AddCard.ActivityAddCardModel }) => void;

        // controller
        activity: AddCard.ActivityAddCardModel = new AddCard.ActivityAddCardModel();

        constructor(private dataAccessService: Antares.Services.DataAccessService) {
            this.dataAccessService.getContactResource();
        }

        $onInit = () => {
            this.dataAccessService.getContactResource().get
        }

        setVendors = (vendors: Business.Contact[]): void => {
            this.activity.contacts = [];
            this.activity.contacts.push.apply(this.activity.contacts, vendors);
        }

        save = () => {
            this.onSave({
                activity: this.activity
            });
        }
    }

    angular.module('app').controller('ActivityAddCardController', ActivityAddCardController);
}