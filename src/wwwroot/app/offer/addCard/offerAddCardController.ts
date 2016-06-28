/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class OfferAddCardController {
        // bindings
        config: IOfferAddPanelConfig;
        onSave: (obj: { offer: Business.CreateOfferCommand }) => void;
        onCancel: () => void;
        onReloadConfig: (obj: { offer: Business.CreateOfferCommand }) => void;
        pristineFlag: any;
        activity: Dto.IActivity;

        // controller
        offerAddCardForm: ng.IFormController;
        offer: Business.CreateOfferCommand = new Business.CreateOfferCommand();
        private defaultOfferStatusCode: string = Common.Models.Enums.OfferStatus[Common.Models.Enums.OfferStatus.New];
        today: Date = new Date();

        // controls
        controlSchemas: any = {
            price: <Attributes.IPriceEditControlSchema> {
                formName : "offerPriceControlForm",
                controlId : "offer-price",
                translationKey : "OFFER.ADD.OFFER",
                fieldName: "offerPrice"
            },
            pricePerWeek: <Attributes.IPriceEditControlSchema> {
                formName: "offerPricePerWeekControlForm",
                controlId: "offer-price-per-week",
                translationKey: "OFFER.ADD.OFFER",
                fieldName: "pricePerWeek",
                suffix: "OFFER.ADD.OFFER_PER_WEEK"
            },
            offerDate: <Attributes.IDateEditControlSchema>{
                formName: "offerDateControlForm",
                controlId: "offer-date",
                translationKey: "OFFER.ADD.OFFER_DATE",
                fieldName: "offerDate"
            },
            exchangeDate: <Attributes.IDateEditControlSchema>{
                formName: "exchangeDateControlForm",
                controlId: "offer-exchange-date",
                translationKey: "OFFER.ADD.EXCHANGE_DATE",
                fieldName: "exchangeDate"
            },
            completionDate: <Attributes.IDateEditControlSchema>{
                formName: "completionDateControlForm",
                controlId: "offer-completion-date",
                translationKey: "OFFER.ADD.COMPLETION_DATE",
                fieldName: "completionDate"
            }
        }

        constructor(
            private enumProvider: Providers.EnumProvider) {
        }

        $onInit = () => {
            this.resetCardData();
        }

        $onChanges = (obj: any) => {
            if (obj.pristineFlag && obj.pristineFlag.currentValue !== obj.pristineFlag.previousValue) {
                this.resetCardData();
                if (this.offerAddCardForm) {
                    this.offerAddCardForm.$setPristine();
                }
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
            this.offer = new Business.CreateOfferCommand();
            this.setDefaultOfferStatus();
        }

        private setDefaultOfferStatus = (): void =>{
            var offerStatus = this.enumProvider.enums[Dto.EnumTypeCode.OfferStatus];
            var defaultOfferStatus: any = _.find(offerStatus, { 'code' : this.defaultOfferStatusCode });
            if (defaultOfferStatus) {
                this.offer.statusId = defaultOfferStatus.id;
            }
        }
    }

    angular.module('app').controller('OfferAddCardController', OfferAddCardController);
}