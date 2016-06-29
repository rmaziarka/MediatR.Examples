module Antares.Offer {
    export class OfferUpdatedSidePanelEvent extends Core.Event {

        constructor(public updatedOffer?: Common.Models.Dto.IOffer) {
            super();
        } 

        getKey(): string{ return "sidepanel.offerUpdated"; }
    }
}