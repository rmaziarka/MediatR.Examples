module Antares.Attributes.Offer {
    import Event = Core.Event;

    export class RequirementUpdatedOfferChainsEvent extends Event {
        private requirementUpdatedOfferChainsEvent: boolean;

        constructor(public updatedRequirement?: Common.Models.Dto.IRequirement){
            super();
        }

        getKey(): string { return "offerChains.requirementUpdated"; }
    }
}