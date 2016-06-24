module Antares.Offer {
    export class OfferAddedSidePanelEvent extends Core.Event {

        constructor(public addedOffer?: Common.Models.Dto.IOffer) {
            super();
        } 

        getKey(): string{ return "sidepanel.offerAdded"; }
    }
}