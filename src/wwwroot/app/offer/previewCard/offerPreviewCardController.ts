/// <reference path="../../typings/_all.d.ts" />

module Antares.Viewing.Preview {
    import Business = Common.Models.Business;

    export class OfferPreviewCardController {
        // binding
        offer: Business.Offer;
    }

    angular.module('app').controller('OfferPreviewCardController', OfferPreviewCardController);
}