module Antares.Common.Component {
    import BaseMessage = Antares.Core.BaseMessage;

    export class CloseSidePanelMessage extends BaseMessage {
        private closeSidePanelMessage: boolean;
        getKey(): string { return "sidepanel.close"; }
    }
}