module Antares.Common.Component {
    import Event = Antares.Core.Event;

    export class BusySidePanelEvent extends Event {
        constructor(public isBusy?: boolean) {
            super();
        }

        getKey(): string{ return "sidepanel.busy"; }
    }
}