/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Offer = Antares.Common.Models.Business.Offer;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class OfferPreviewPanelController extends Antares.Common.Component.BaseSidePanelController {
        // binding
        offer: Offer;
    }

    angular.module('app').controller('OfferPreviewPanelController', OfferPreviewPanelController);
}