/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class OfferEditCardController {
        // bindings
        config: IOfferEditPanelConfig;
        onSave: (obj: { offer: Dto.IOffer }) => void;
        onCancel: () => void;
        pristineFlag: any;
        offer: Dto.IOffer;
        backToPreview: boolean;

        // controller
        offerEditCardForm: ng.IFormController;
        originalOffer: Dto.IOffer;
        today: Date = new Date();

        constructor(
            private $state: ng.ui.IStateService,
            private appConfig: Common.Models.IAppConfig,
            private $window: ng.IWindowService){
        }

        // controls
        controlSchemas: any = {
            status: <Attributes.IEnumItemEditControlSchema>{
                formName: "offerStatusControlForm",
                controlId: "offer-status",
                translationKey: "OFFER.EDIT.STATUS",
                fieldName: "statusId",
                enumTypeCode: Dto.EnumTypeCode.OfferStatus
            },
            price: <Attributes.IPriceEditControlSchema>{
                formName: "offerPriceControlForm",
                controlId: "offer-price",
                translationKey: "OFFER.EDIT.OFFER",
                fieldName: "price"
            },
            pricePerWeek: <Attributes.IPriceEditControlSchema>{
                formName: "offerPricePerWeekControlForm",
                controlId: "offer-price-per-week",
                translationKey: "OFFER.EDIT.OFFER",
                fieldName: "pricePerWeek",
                suffix: "OFFER.EDIT.OFFER_PER_WEEK"
            },
            offerDate: <Attributes.IDateEditControlSchema>{
                formName: "offerDateControlForm",
                controlId: "offer-date",
                translationKey: "OFFER.EDIT.OFFER_DATE",
                fieldName: "offerDate"
            },
            exchangeDate: <Attributes.IDateEditControlSchema>{
                formName: "exchangeDateControlForm",
                controlId: "offer-exchange-date",
                translationKey: "OFFER.EDIT.EXCHANGE_DATE",
                fieldName: "exchangeDate"
            },
            completionDate: <Attributes.IDateEditControlSchema>{
                formName: "completionDateControlForm",
                controlId: "offer-completion-date",
                translationKey: "OFFER.EDIT.COMPLETION_DATE",
                fieldName: "completionDate"
            },
            specialConditions: <Attributes.ITextEditControlSchema>{
                formName: "specialConfitionsControlForm",
                controlId: "offer-special-conditions",
                translationKey: "OFFER.EDIT.SPECIAL_CONDITIONS",
                fieldName: "specialConditions"
            }
        }

        $onChanges = (obj: any) => {
            if (obj.pristineFlag && obj.pristineFlag.currentValue !== obj.pristineFlag.previousValue) {
                this.resetCardData();
                if (this.offerEditCardForm) {
                    this.offerEditCardForm.$setPristine();
                }
            }

            if (obj.offer) {
                this.offer = new Business.Offer(obj.offer.currentValue);
                this.originalOffer = new Business.Offer(obj.offer.currentValue);
            }
        }

        navigateToActivity = (offer: Dto.IOffer) => {
            var activityViewUrl = this.appConfig.appRootUrl + this.$state.href('app.activity-view', { id: offer.activity.id }, { absolute: false });
            this.$window.open(activityViewUrl, '_blank');
        }

        private resetCardData = () => {
            this.offer = this.originalOffer;
        }

        cancel = () => {
            this.onCancel();
        }

        save = () => {
            this.onSave({
                offer: this.offer
            });
        }
    }

    angular.module('app').controller('OfferEditCardController', OfferEditCardController);
}