module Antares.Attributes {
    import Event = Core.Event;

    export class OpenViewingPrewiewPanelEvent extends Event {
        private openViewingPrewiewPanelEvent: boolean;
        getKey(): string { return "viewingPrewiew.openPanel"; }
    }
}