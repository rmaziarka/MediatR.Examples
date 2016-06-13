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
        onCancel: () => void;
        pristineFlag: any;

        // controller
        activity: AddCard.ActivityAddCardModel = new AddCard.ActivityAddCardModel();
        vendorContacts: Business.Contact[];
        activityAddCardForm: ng.IFormController;

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private enumService: Services.EnumService) {
            this.dataAccessService.getContactResource();
        }

        $onChanges = (obj: any) => {
            if (obj.pristineFlag && obj.pristineFlag.currentValue !== obj.previousValue) {
                this.setPristine();
            }
        }

        getVendorContacts = (): Business.Contact[] => {
            var vendor: Business.Ownership = _.find(this.ownerships, (ownership: Business.Ownership) => {
                return ownership.isVendor();
            });

            if (vendor) {
                return vendor.contacts;
            }
        }

        save = () => {
            this.onSave({
                activity: this.activity
            });
        }

        cancel = () => {
            this.onCancel();
        }

        private setPristine = () => {
            this.activityAddCardForm.$setPristine();
        }
    }

    angular.module('app').controller('ActivityAddCardController', ActivityAddCardController);
}