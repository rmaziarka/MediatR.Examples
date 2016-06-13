/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityAddCardController {
        // bindings
        propertyTypeId: string;
        ownerships: Business.Ownership[];
        config: IActivityAddPanelConfig;
        onSave: (obj: { activity: AddCard.ActivityAddCardModel }) => void;

        // controller
        activity: AddCard.ActivityAddCardModel = new AddCard.ActivityAddCardModel();
        vendorContacts: Business.Contact[];

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private enumService: Services.EnumService) {
            this.dataAccessService.getContactResource();
        }

        $onInit = () => {
            this.setVendors();
        }

        $onChanges = (obj: any) => {
            // TODO change ownerships to immutable list
            if (obj.ownerships.currentValue !== obj.ownerships.previousValue) {
                this.setVendors();
            }
        }

        setVendors = (): void => {
            var vendor: Business.Ownership = _.find(this.ownerships, (ownership: Business.Ownership) => {
                return ownership.isVendor();
            });

            if (vendor) {
                this.vendorContacts = vendor.contacts;
            }
        }

        save = () => {
            this.onSave({
                activity: this.activity
            });
        }
    }

    angular.module('app').controller('ActivityAddCardController', ActivityAddCardController);
}