module Antares.Attributes.Offer {
    import Event = Core.Event;

    export class OpenChainPanelEvent extends Event {
        private openChainPanelEvent: boolean;
        getKey(): string { return "offerChains.openPanel"; }
    }
}