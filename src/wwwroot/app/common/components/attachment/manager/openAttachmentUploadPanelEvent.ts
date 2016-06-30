module Antares.Common.Component.Attachment {
    import Event = Core.Event;

    export class OpenAttachmentUploadPanelEvent extends Event {
        private openAttachmentUploadPanelEvent: boolean;
        getKey(): string { return "attachmentUpload.openPanel"; }
    }
}