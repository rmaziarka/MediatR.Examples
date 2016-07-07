/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    import Enums = Common.Models.Enums;

    export class OfferChainsControlController {
        // bindings
        isPanelVisible: Enums.SidePanelState;
        chainCommand: any;
        config: IOfferChainsControlConfig;
        chainType: Enums.OfferChainsType;
    }

    angular.module('app').controller('OfferChainsControlController', OfferChainsControlController);
}