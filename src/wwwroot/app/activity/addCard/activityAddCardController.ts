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
        onReloadConfig: (obj: { activity: AddCard.ActivityAddCardModel }) => void;
        pristineFlag: any;

        // controller
        activity: AddCard.ActivityAddCardModel = new AddCard.ActivityAddCardModel();
        activityAddCardForm: ng.IFormController;
        private defaultActivityStatusCode: string = 'PreAppraisal';

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private enumService: Services.EnumService) {
            this.dataAccessService.getContactResource();
        }

        $onInit = () => {
            this.resetCardData();
        }

        $onChanges = (obj: any) => {
            if (obj.ownerships && obj.ownerships.currentValue !== obj.ownerships.previousValue) {
                this.setVendorContacts();
            }

            if (obj.pristineFlag && obj.pristineFlag.currentValue !== obj.previousValue) {
                this.resetCardData();
                this.activityAddCardForm.$setPristine();
            }
        }

        public save = () => {
            this.onSave({
                activity: this.activity
            });
        }

        public cancel = () => {
            this.onCancel();
        }

        private setVendorContacts = (): void => {
            var vendor: Business.Ownership = _.find(this.ownerships, (ownership: Business.Ownership) => {
                return ownership.isVendor();
            });

            if (vendor) {
                this.activity.contacts = vendor.contacts;
            }
        }

        reloadConfig = () => {
            this.onReloadConfig({
                activity: this.activity
            });
        }

        private setDefaultActivityStatus = (): void => {
            this.enumService.getEnumPromise().then((enumStatuses: Dto.IEnumDictionary) => {
                var activityStatuses = enumStatuses[Dto.EnumTypeCode.ActivityStatus];
                var defaultActivityStatus: any = _.find(activityStatuses, { 'code': this.defaultActivityStatusCode });

                if (defaultActivityStatus) {
                    this.activity.activityStatusId = defaultActivityStatus.id;
                }
            });
        }

        private resetCardData = () => {
            this.activity = new AddCard.ActivityAddCardModel();

            this.setVendorContacts();
            this.setDefaultActivityStatus();
        }
    }

    angular.module('app').controller('ActivityAddCardController', ActivityAddCardController);
}