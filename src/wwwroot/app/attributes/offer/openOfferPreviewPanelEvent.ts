module Antares.Attributes {
    import Event = Core.Event;

    export class OpenOfferPreviewPanelEvent extends Event {
        private openOfferPreviewPanelEvent: boolean;
        getKey(): string { return "offerPrewiew.openPanel"; }
    }
}