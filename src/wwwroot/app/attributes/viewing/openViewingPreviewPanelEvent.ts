module Antares.Attributes {
    import Event = Core.Event;

    export class OpenViewingPreviewPanelEvent extends Event {
        private openViewingPreviewPanelEvent: boolean;
        getKey(): string { return "viewingPreview.openPanel"; }
    }
}