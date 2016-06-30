module Antares.Common.Component.Attachment {
    import Event = Core.Event;

    export class OpenAttachmentPreviewPanelEvent extends Event {
        private openAttachmentPreviewPanelEvent: boolean;
        getKey(): string { return "attachmentPreview.openPanel"; }
    }
}