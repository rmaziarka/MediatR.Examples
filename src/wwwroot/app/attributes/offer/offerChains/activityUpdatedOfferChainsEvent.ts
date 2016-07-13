module Antares.Attributes.Offer {
    import Event = Core.Event;

    export class ActivityUpdatedOfferChainsEvent extends Event {
        private activityUpdatedOfferChainsEvent: boolean;

        constructor(public updatedActivity?: Common.Models.Dto.IActivity){
            super();
        }

        getKey(): string { return "offerChains.activityUpdated"; }
    }
}