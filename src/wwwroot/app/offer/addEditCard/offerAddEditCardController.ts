/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class OfferAddEditCardController {
        // bindings
        config: IOfferAddEditPanelConfig;
        onSave: (obj: { offer: Business.Offer }) => void;
        onCancel: () => void;
        pristineFlag: any;
        activity: Dto.IActivity;

        // controller
        offerAddEditCardForm: ng.IFormController;
        offer: Business.Offer = new Business.Offer();

        constructor(
            private enumService: Services.EnumService){
        }

        $onInit = () => {
            this.resetCardData();
        }

        $onChanges = (obj: any) => {
            if (obj.pristineFlag && obj.pristineFlag.currentValue !== obj.previousValue) {
                this.resetCardData();
                this.offerAddEditCardForm.$setPristine();
            }
        }

        public save = () => {
            this.onSave({
                offer: this.offer
            });
        }

        public cancel = () => {
            this.onCancel();
        }

        private resetCardData = () => {

        }
    }

    angular.module('app').controller('OfferAddEditCardController', OfferAddEditCardController);
}