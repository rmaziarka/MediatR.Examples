/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;

    export class OfferPreviewCardController {
        // bindings
        config: IOfferEditPanelConfig;
        offer: Dto.IOffer;
        onEdit: () => void;

        constructor(
            private $state: ng.ui.IStateService,
            private appConfig: Common.Models.IAppConfig,
            private $window: ng.IWindowService) {
        }

        // controls
        controlSchemas: any = {
            status: <Attributes.IEnumItemControlSchema> {
                controlId: "offer-status",
                translationKey: "OFFER.PREVIEW.STATUS",
                enumTypeCode: Dto.EnumTypeCode.OfferStatus
            },
            price: <Attributes.IPriceControlSchema> {
                controlId: "offer-price",
                translationKey: "OFFER.PREVIEW.OFFER"
            },
            pricePerWeek: <Attributes.IPriceControlSchema> {
                controlId: "offer-price-per-week",
                translationKey: "OFFER.PREVIEW.OFFER",
                suffix: "OFFER.PREVIEW.OFFER_PER_WEEK"
            },
            offerDate: <Attributes.IDateControlSchema> {
                controlId: "offer-date",
                translationKey: "OFFER.PREVIEW.OFFER_DATE"
            },
            exchangeDate: <Attributes.IDateControlSchema> {
                controlId: "offer-exchange-date",
                translationKey: "OFFER.PREVIEW.EXCHANGE_DATE"
            },
            completionDate: <Attributes.IDateControlSchema> {
                controlId: "offer-completion-date",
                translationKey: "OFFER.PREVIEW.COMPLETION_DATE",
            },
            specialConditions: <Attributes.ITextControlSchema> {
                controlId: "offer-special-conditions",
                translationKey: "OFFER.PREVIEW.SPECIAL_CONDITIONS"
            }
        }

        edit = () => {
            this.onEdit();
        }

        navigateToActivity = (activity: Dto.IActivity) => {
            var activityViewUrl = this.appConfig.appRootUrl + this.$state.href('app.activity-view', { id: activity.id }, { absolute: false });
            this.$window.open(activityViewUrl, '_blank');
        }
    }

    angular.module('app').controller('OfferPreviewCardController', OfferPreviewCardController);
}