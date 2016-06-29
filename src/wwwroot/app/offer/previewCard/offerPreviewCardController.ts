/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;

    export class OfferPreviewCardController {
        // bindings
        config: IOfferEditPanelConfig;
        offer: Dto.IOffer;
        onEdit: () => void;

        edit = () => {
            this.onEdit();
        }
    }

    angular.module('app').controller('OfferPreviewCardController', OfferPreviewCardController);
}