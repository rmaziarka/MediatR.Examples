module Antares.Common.Component.Attachment {
    import Event = Antares.Core.Event;

    export class AttachmentSavedEvent extends Event {
        constructor(public attachmentSaved?: Antares.Common.Models.Dto.IAttachment) {
            super();
        }

        getKey(): string { return "sidepanel.attachmentSave"; }
    }
}