module Antares.Common.Component {
    import Event = Antares.Core.Event;

    export class CloseSidePanelEvent extends Event {
        private closeSidePanelEvent: boolean;
        getKey(): string{ return "sidepanel.close"; }
    }
}