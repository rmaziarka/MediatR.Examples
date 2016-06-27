module Antares.Activity {
    import BaseMessage = Antares.Core.Event;

    export class ActivityAddedSidePanelEvent extends Core.Event {

        constructor(public activityAdded?: Antares.Common.Models.Dto.IActivity) {
            super();
        } 

        getKey(): string { return "sidepanel.activityAdded"; }
    }
}