module Antares.Activity {
    import BaseMessage = Antares.Core.BaseMessage;

    export class ActivityAddedSidePanelMessage extends BaseMessage {

        constructor(public activityAdded: Antares.Common.Models.Dto.IActivity) {
            super();
        } 

        getKey(): string { return "sidepanel.activityAdded"; }
    }
}